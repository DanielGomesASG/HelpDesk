using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Net.Mail;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Configuration.Tenants;
using DanielASG.HelpDesk.Net.Emailing;
using DanielASG.HelpDesk.Url;

namespace DanielASG.HelpDesk.Tickets
{
    public class TicketEmailer : HelpDeskAppServiceBase, ITicketEmailer, ITransientDependency
    {
        public IAppUrlService AppUrlService { get; set; }

        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<Status> _statusRepository;
        private readonly IRepository<TicketMessage> _ticketMessageRepository;

        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly ISettingManager _settingManager;

        private readonly TenantSettingsAppService _tenantSettingsAppService;

        private string _emailButtonStyle =
            "padding-left: 30px; padding-right: 30px; padding-top: 12px; padding-bottom: 12px; color: #ffffff; background-color: #00bb77; font-size: 14pt; text-decoration: none;";

        private string _emailButtonColor = "#00bb77";

        public TicketEmailer(IRepository<Ticket> ticketRepository,
                             IRepository<Status> statusRepository,
                             IRepository<TicketMessage> ticketMessageRepository,

                             IEmailSender emailSender,
                             IEmailTemplateProvider emailTemplateProvider,
                             ISettingManager settingManager,

                             TenantSettingsAppService tenantSettingsAppService)
        {
            _ticketRepository = ticketRepository;
            _statusRepository = statusRepository;
            _ticketMessageRepository = ticketMessageRepository;

            _emailSender = emailSender;
            _emailTemplateProvider = emailTemplateProvider;
            _settingManager = settingManager;

            _tenantSettingsAppService = tenantSettingsAppService;
        }

        public async Task SendStatusEmail(int tenantId, int ticketId, int oldStatusId, int newStatusId)
        {
            var mailMessage = CreateStatusMail(tenantId, ticketId, oldStatusId, newStatusId).Result;

            if (mailMessage != null)
            {
                try
                {
                    await _emailSender.SendAsync(mailMessage);
                }
                catch (Exception e)
                {
                    throw new UserFriendlyException("An error was encountered while sending an email. " + e.Message, e);
                }
            }
        }

        private async Task<MailMessage> CreateStatusMail(int tenantId, int ticketId, int oldStatusId, int newStatusId)
        {
            var ticket = await _ticketRepository.GetAll().AsNoTracking()
                                                .Where(x => x.Id == ticketId)
                                                .Include(x => x.CustomerUser)
                                                .FirstOrDefaultAsync();
            var oldStatus = await _statusRepository.GetAsync(oldStatusId);
            var newStatus = await _statusRepository.GetAsync(newStatusId);

            var changeDate = DateTime.Now;

            var emailTemplate = GetTitleAndSubTitle(tenantId, L("ChangeStatusEmail_Title"), L("ChangeStatusEmail_SubTitle"));

            var link = AppUrlService.CreateTicketsUrlFormat(tenantId);

            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<h3>" + L("Ticket") + " " + ticket.Id + "</h3> " + "<br />");
            mailMessage.AppendLine("<b>" + L("OldStatus") + "</b>: " + oldStatus.Name + "<br />");
            mailMessage.AppendLine("<b>" + L("NewStatus") + "</b>: " + newStatus.Name + "<br />");
            mailMessage.AppendLine("<b>" + L("ChangeDate") + "</b>: " + changeDate.ToString("dd/MM/yyyy HH:mm:ss") + "<br />");
            mailMessage.AppendLine("<b>" + L("NewStatusDescription") + "</b>: " + newStatus.Description + "<br />");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine(L("ClickTheLinkBelowToSeeTicketInfos") + "<br /> <br />");
            mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor +
                                   "\" href=\"" + link + "\">" + L("MyTickets") + "</a>" + "<br />");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" +
                                       L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
            mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");
            mailMessage.AppendLine("<br />");


            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

            return new MailMessage
            {
                To = { ticket.CustomerUser.EmailAddress },
                Subject = L("ChangeStatusEmail_Subject"),
                Body = emailTemplate.ToString(),
                IsBodyHtml = true
            };
        }

        public async Task SendDefaultMessageEmail(int tenantId, int ticketId, string newMessage)
        {
            var mailMessage = CreateDefaultMessageMail(tenantId, ticketId, newMessage).Result;

            if (mailMessage != null)
            {
                try
                {
                    await _emailSender.SendAsync(mailMessage);
                }
                catch (Exception e)
                {
                    throw new UserFriendlyException("An error was encountered while sending an email. " + e.Message, e);
                }
            }
        }

        private async Task<MailMessage> CreateDefaultMessageMail(int tenantId, int ticketId, string newMessage)
        {
            var ticket = await _ticketRepository.GetAll().AsNoTracking()
                                                .Where(x => x.Id == ticketId)
                                                .Include(x => x.CustomerUser)
                                                .FirstOrDefaultAsync();

            var emailTemplate = GetTitleAndSubTitle(tenantId, L("DefaultMessageEmail_Title"), L("DefaultMessageEmail_SubTitle"));

            var link = AppUrlService.CreateTicketsUrlFormat(tenantId);

            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<h3>" + L("Ticket") + " " + ticket.Id + "</h3> " + "<br />");
            mailMessage.AppendLine("<b>" + L("Date") + "</b>: " +
                                       DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") +
            "<br />");
            mailMessage.AppendLine("<b>" + L("DefaultMessage") + "</b>: " + newMessage + "<br />");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine(L("ClickTheLinkBelowToSeeTicketInfos") + "<br /> <br />");
            mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor +
                                   "\" href=\"" + link + "\">" + L("MyTickets") + "</a>" + "<br />");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" +
                                       L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
            mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");
            mailMessage.AppendLine("<br />");


            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

            return new MailMessage
            {
                To = { ticket.CustomerUser.EmailAddress },
                Subject = L("DefaultMessageEmail_Subject"),
                Body = emailTemplate.ToString(),
                IsBodyHtml = true
            };
        }

        public async Task SendMessageEmail(int tenantId, int ticketId, string newMessage)
        {
            var mailMessage = CreateMessageMail(tenantId, ticketId, newMessage).Result;

            if (mailMessage != null)
            {
                try
                {
                    await _emailSender.SendAsync(mailMessage);
                }
                catch (Exception e)
                {
                    throw new UserFriendlyException("An error was encountered while sending an email. " + e.Message, e);
                }
            }
        }

        private async Task<MailMessage> CreateMessageMail(int tenantId, int ticketId, string newMessage)
        {
            var ticket = await _ticketRepository.GetAll().AsNoTracking()
                                                .Where(x => x.Id == ticketId)
                                                .Include(x => x.CustomerUser)
                                                .FirstOrDefaultAsync();

            var emailTemplate = GetTitleAndSubTitle(tenantId, L("MessageEmail_Title"), L("Ticket") + " " + ticket.Id);

            var link = AppUrlService.CreateTicketsUrlFormat(tenantId);

            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + ticket.CustomerUser.Name + "</b>: " + newMessage + "<br />");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine(L("ClickTheLinkBelowToSeeTicketInfos") + "<br /> <br />");
            mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor +
                                   "\" href=\"" + link + "\">" + L("MyTickets") + "</a>" + "<br />");
            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" +
                                       L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
            mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");
            mailMessage.AppendLine("<br />");


            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

            MailMessage mail = new MailMessage()
            {
                To = { ticket.CustomerUser.EmailAddress },
                Subject = "Re: " + ticket.Subject,
                Body = emailTemplate.ToString(),
                IsBodyHtml = true
            };

            var messages = await _ticketMessageRepository.GetAll()
                                                         .Where(x => x.TicketId == ticketId)
                                                         .OrderBy(x => x.CreationTime)
                                                         .ToListAsync();

            var defaultAddress = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.DefaultFromAddress, tenantId);
            if (defaultAddress.IsNullOrEmpty())
                defaultAddress = await _settingManager.GetSettingValueForTenantAsync(EmailSettingNames.DefaultFromAddress, 1);

            defaultAddress = defaultAddress[defaultAddress.IndexOf('@')..];

            var customMessageId = $"{Guid.NewGuid() + defaultAddress}";

            messages[^1].UniqueId = customMessageId;

            var messageUids = messages.Select(x => x.UniqueId).ToList();
            messageUids.RemoveAt(messageUids.Count - 1);

            mail.Headers.Add("Message-ID", $"<{customMessageId}>");
            mail.Headers.Add("In-Reply-To", $"<{messageUids[^1]}>");

            var references = string.Join(" ", messageUids.Select(uid => $"<{uid}>"));
            mail.Headers.Add("References", references);

            return mail;
        }

        private StringBuilder GetTitleAndSubTitle(int tenantId, string title, string subTitle)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(tenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", title);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", subTitle);

            return emailTemplate;
        }
    }
}
