using Abp;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Net.Mail;
using Abp.ObjectMapping;
using Abp.Runtime.Security;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using DanielASG.HelpDesk.Authorization.Roles;
using DanielASG.HelpDesk.Authorization.Users;
using DanielASG.HelpDesk.Configuration;
using DanielASG.HelpDesk.MultiTenancy;
using DanielASG.HelpDesk.Notifications;
using DanielASG.HelpDesk.Tickets.Dtos;
using DanielASG.HelpDesk.Url;

namespace DanielASG.HelpDesk.Tickets.Worker
{
    public class TicketEmailWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        public bool IsEnabled { get; }

        private const int CheckPeriodAsMilliseconds = 1 * 1000 * 60 * 1; // 1min

        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<TicketMessage> _ticketMessageRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Status> _statusRepository;

        protected readonly IWebUrlService _webUrlService;
        private readonly IAppConfigurationAccessor _configurationAccessor;
        private readonly IObjectMapper ObjectMapper;
        private readonly ISettingManager _settingManager;
        private readonly IAppNotifier _appNotifier;

        private readonly RoleManager _roleManager;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly UserManager UserManager;
        public TicketEmailWorker(
            AbpTimer timer,
            IRepository<Tenant> tenantRepository,
            IRepository<Ticket> ticketRepository,
            IRepository<TicketMessage> ticketMessageRepository,
            IRepository<User, long> userRepository,
            IRepository<Status> statusRepository,

            IWebUrlService webUrlService,
            IAppConfigurationAccessor configurationAccessor,
            IObjectMapper objectMapper,
            ISettingManager settingManager,
            IAppNotifier appNotifier,

            RoleManager roleManager,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,
            UserManager userManager

        )
            : base(timer)
        {
            _tenantRepository = tenantRepository;
            _ticketRepository = ticketRepository;
            _ticketMessageRepository = ticketMessageRepository;
            _userRepository = userRepository;
            _statusRepository = statusRepository;

            _webUrlService = webUrlService;
            _configurationAccessor = configurationAccessor;
            ObjectMapper = objectMapper;
            _settingManager = settingManager;
            _appNotifier = appNotifier;

            _roleManager = roleManager;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;

            Timer.Period = CheckPeriodAsMilliseconds;
            Timer.RunOnStart = true;

            IsEnabled = configurationAccessor.Configuration["TicketEmailWorker:IsEnabled"] ==
                        true.ToString();
            UserManager = userManager;
        }

        protected override void DoWork()
        {
            if (!IsEnabled) return;

            List<int> tenantIds;
            using (var uow = UnitOfWorkManager.Begin())
            {
                tenantIds = _tenantRepository.GetAll()
                    .Select(t => t.Id)
                    .ToList();

                uow.Complete();
            }

            foreach (var tenantId in tenantIds)
            {
                CheckNewEmails(tenantId);
            }
        }

        protected virtual async void CheckNewEmails(int tenantId)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete, CustomFilterConsts.IsActive))
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            using (var client = new ImapClient())
            {
                try
                {
                    var host = _settingManager.GetSettingValueForTenant(EmailSettingNames.Smtp.Host, tenantId).Replace("smtp", "imap");
                    var username = _settingManager.GetSettingValueForTenant(EmailSettingNames.Smtp.UserName, tenantId);
                    var smtpPassword = _settingManager.GetSettingValueForTenant(EmailSettingNames.Smtp.Password, tenantId);
                    var password = SimpleStringCipher.Instance.Decrypt(smtpPassword);

                    await client.ConnectAsync(host, 993, true);
                    await client.AuthenticateAsync(username, password);
                    var inbox = client.Inbox;
                    await inbox.OpenAsync(FolderAccess.ReadOnly);

                    var indexes = await inbox.SearchAsync(SearchQuery.NotSeen);

                    foreach (var index in indexes)
                    {
                        var message = await inbox.GetMessageAsync(index);
                        Logger.Warn($"Message-ID: {message.MessageId}\n");

                        await CreateOrEditTicket(message, tenantId);
                    }

                    await client.DisconnectAsync(true);
                    uow.Complete();
                }
                catch
                {
                    await client.DisconnectAsync(true);
                    Logger.Warn($"Tenant email connect fail: {tenantId}\n");
                    uow.Complete();
                    return;
                }
            }
        }

        private async Task CreateOrEditTicket(MimeMessage message, int tenantId)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete, CustomFilterConsts.IsActive))
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var ticketMessage = _ticketMessageRepository.FirstOrDefault(x => x.UniqueId == message.MessageId);

                Logger.Warn($"In Reply To: {message.InReplyTo}\n");
                Logger.Warn($"Ticket Message: {ticketMessage}\n");

                if (ticketMessage != null) { uow.Complete(); return; }

                if (message.InReplyTo == null)
                {
                    await CreateTicket(message, tenantId);
                }
                else
                {
                    await UpdateTicket(message, tenantId);
                }

                uow.Complete();
            }
        }

        private async Task CreateTicket(MimeMessage message, int tenantId)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete, CustomFilterConsts.IsActive))
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var messageText = RemoveQuotedText(message.TextBody);
                var sender = message.From.Mailboxes.FirstOrDefault();
                var customer = await _userRepository.FirstOrDefaultAsync(x => x.EmailAddress == sender.Address);

                if (customer.IsNullOrDeleted())
                {
                    customer = await CreateCustomer(sender, tenantId);
                }

                var dto = new CreateOrEditTicketDto()
                {
                    Origin = EOrigin.Email,
                    MessageUniqueId = message.MessageId,
                    Priority = EPriority.Normal,
                    Subject = message.Subject,
                    Message = messageText,
                    Notify = true,
                    CustomerUserId = customer.Id,
                };

                Logger.Warn($"Ticket Dto: {dto}\n");
                var ticket = ObjectMapper.Map<Ticket>(dto);

                if (!ticket.StatusId.HasValue)
                {
                    var status = await _statusRepository.FirstOrDefaultAsync(x => x.Code == EStatus.AguardandoAtendente);
                    ticket.StatusId = status.Id;
                }

                ticket.TenantId = tenantId;

                await _ticketRepository.InsertAsync(ticket);
                await CurrentUnitOfWork.SaveChangesAsync();
                Logger.Warn($"New Ticket: {ticket}\n");

                var usersList = await _userRepository.GetAllList()
                                                     .Where(x => x.ProfileType == EProfileType.Staff || x.ProfileType == EProfileType.Internal)
                                                     .ToDynamicListAsync();

                var users = new List<UserIdentifier>();
                usersList.ForEach(x => users.Add(x.ToUserIdentifier()));

                Logger.Warn($"Users Count: {users.Count}\n");
                await _appNotifier.SendMessageAsync(
                    "Novo chamado por email!",
                    "Novo chamado por email adicionado ao sistema! Acesse a aba de tickets para mais detalhes.",
                    users.ToArray(),
                    Abp.Notifications.NotificationSeverity.Success);

                await CurrentUnitOfWork.SaveChangesAsync();

                await _ticketMessageRepository.InsertAsync(new TicketMessage()
                {
                    TenantId = tenantId,
                    Message = dto.Message,
                    TicketId = ticket.Id,
                    Origin = dto.Origin,
                    UniqueId = dto.MessageUniqueId,
                    CreatorUserId = customer.Id,
                });

                await CurrentUnitOfWork.SaveChangesAsync();
                uow.Complete();
            }
        }

        private async Task UpdateTicket(MimeMessage message, int tenantId)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete, CustomFilterConsts.IsActive))
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var messageText = RemoveQuotedText(message.TextBody);
                var ticketMessage = await _ticketMessageRepository.FirstOrDefaultAsync(x => x.UniqueId == message.InReplyTo);
                var ticket = await _ticketRepository.FirstOrDefaultAsync(x => x.Id == ticketMessage.TicketId);

                if (ticketMessage == null)
                {
                    Logger.Warn($"Update Ticket FAIL: Ticket message cannot be null! UID: {message.MessageId}\n");
                    uow.Complete();
                    return;
                }

                await _ticketMessageRepository.InsertAsync(new TicketMessage()
                {
                    TenantId = tenantId,
                    Message = messageText,
                    TicketId = ticketMessage.TicketId,
                    Origin = EOrigin.Email,
                    UniqueId = message.MessageId,
                    CreatorUserId = ticket.CustomerUserId,
                });

                await CurrentUnitOfWork.SaveChangesAsync();
                Logger.Warn($"Update Ticket: {ticketMessage}\n");
                uow.Complete();
            }
        }

        private async Task<User> CreateCustomer(MailboxAddress sender, int tenantId)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete, CustomFilterConsts.IsActive))
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var user = new User()
                {
                    TenantId = tenantId,
                    Name = sender.Name,
                    Surname = "",
                    EmailAddress = sender.Address,
                    Password = "123qwe",
                    UserName = sender.Address.Remove(sender.Address.IndexOf('@')),
                    ProfileType = EProfileType.Customer,
                    ShouldChangePasswordOnNextLogin = true,
                };

                Logger.Warn($"New User: {user}\n");
                await UserManager.InitializeOptionsAsync(tenantId);
                foreach (var validator in _passwordValidators)
                {
                    CheckErrors(await validator.ValidateAsync(UserManager, user, user.Password));
                }

                user.Password = _passwordHasher.HashPassword(user, user.Password);

                CheckErrors(await UserManager.CreateAsync(user));
                await CurrentUnitOfWork.SaveChangesAsync();

                user.Roles = new Collection<UserRole>();
                var role = await _roleManager.GetRoleByNameAsync("User");
                user.Roles.Add(new UserRole(tenantId, user.Id, role.Id));

                await _appNotifier.WelcomeToTheApplicationAsync(user);

                await CurrentUnitOfWork.SaveChangesAsync();

                uow.Complete();
                return user;
            }
        }

        private static string RemoveQuotedText(string emailBody)
        {
            bool isHtml = emailBody.Contains("<html");

            if (isHtml)
            {
                emailBody = RemoveHtmlQuotedText(emailBody);
            }
            else
            {
                emailBody = RemovePlainTextQuotedText(emailBody);
            }

            return emailBody.Trim();
        }

        private static string RemovePlainTextQuotedText(string emailBody)
        {
            emailBody = Regex.Replace(emailBody, @"\[\s*image:.*?\]", string.Empty, RegexOptions.Singleline);
            emailBody = Regex.Replace(emailBody, @"https?://\S+", string.Empty, RegexOptions.Singleline);
            emailBody = Regex.Replace(emailBody, @"\r\n", " ");
            emailBody = Regex.Replace(emailBody, @"\n+", " ");

            string[] delimiters =
            [
                @"(?i)On\s.*wrote:",
                @"(?i)Em\s.*escreveu:",
                @"(?i)--\sForwarded message\s--",
                @"(?i)Original message",
            ];

            foreach (var delimiter in delimiters)
            {
                var regex = new Regex(delimiter);
                if (regex.IsMatch(emailBody))
                {
                    emailBody = regex.Split(emailBody)[0];
                    break;
                }
            }

            return emailBody.Trim();
        }

        private static string RemoveHtmlQuotedText(string emailBody)
        {
            string[] htmlDelimiters =
            [
                @"<blockquote.*?>.*?</blockquote>",
                @"<div\s+class=[""']gmail_quote[""'].*?>.*?</div>",
                @"<div\s+class=[""']yahoo_quoted[""'].*?>.*?</div>",
                @"<div\s+class=[""'].*?quoted.*?[""'].*?>.*?</div>"
            ];

            foreach (var htmlDelimiter in htmlDelimiters)
            {
                var regex = new Regex(htmlDelimiter, RegexOptions.Singleline);
                emailBody = regex.Replace(emailBody, string.Empty);
            }

            emailBody = Regex.Replace(emailBody, @"<hr\s*/?>", string.Empty, RegexOptions.IgnoreCase);
            emailBody = Regex.Replace(emailBody, @"<br\s*/?>", string.Empty, RegexOptions.IgnoreCase);
            emailBody = Regex.Replace(emailBody, @"<[^>]+>", string.Empty);

            return emailBody.Trim();
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
