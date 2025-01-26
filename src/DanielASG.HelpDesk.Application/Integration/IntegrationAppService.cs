using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Logging;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using DanielASG.HelpDesk.Authorization;
using DanielASG.HelpDesk.Authorization.Impersonation;
using DanielASG.HelpDesk.Authorization.Roles;
using DanielASG.HelpDesk.Authorization.Users;
using DanielASG.HelpDesk.Configuration;
using DanielASG.HelpDesk.Departments;
using DanielASG.HelpDesk.Editions;
using DanielASG.HelpDesk.Integration.Dtos;
using DanielASG.HelpDesk.MultiTenancy;
using DanielASG.HelpDesk.Notifications;
using DanielASG.HelpDesk.Tickets;
using DanielASG.HelpDesk.Url;

namespace DanielASG.HelpDesk.Integration
{
    public class IntegrationAppService : HelpDeskAppServiceBase, IIntegrationAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<MessageType> _messageTypeRepository;
        private readonly IRepository<Status> _statusRepository;
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<Department> _departmentRepository;

        private readonly ImpersonationManager _impersonationManager;
        private readonly LogInManager _logInManager;
        private readonly ICacheManager _cacheManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAppConfigurationAccessor _configurationAccessor;
        private readonly EditionManager _editionManager;

        protected readonly IWebUrlService _webUrlService;

        private readonly RoleManager _roleManager;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;

        public IntegrationAppService(IRepository<User, long> userRepository,
                                     IRepository<Tenant> tenantRepository,
                                     IRepository<MessageType> messageTypeRepository,
                                     IRepository<Status> statusRepository,
                                     IRepository<Ticket> ticketRepository,
                                     IRepository<Department> departmentRepository,

                                     ImpersonationManager impersonationManager,
                                     LogInManager logInManager,
                                     ICacheManager cacheManager,
                                     IAppNotifier appNotifier,
                                     IUnitOfWorkManager unitOfWorkManager,
                                     IAppConfigurationAccessor configurationAccessor,
                                     EditionManager editionManager,

                                     IWebUrlService webUrlService,

                                     RoleManager roleManager,
                                     IEnumerable<IPasswordValidator<User>> passwordValidators,
                                     IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _tenantRepository = tenantRepository;
            _messageTypeRepository = messageTypeRepository;
            _statusRepository = statusRepository;
            _ticketRepository = ticketRepository;
            _departmentRepository = departmentRepository;

            _impersonationManager = impersonationManager;
            _logInManager = logInManager;
            _cacheManager = cacheManager;
            _appNotifier = appNotifier;
            _unitOfWorkManager = unitOfWorkManager;
            _configurationAccessor = configurationAccessor;
            _editionManager = editionManager;

            _webUrlService = webUrlService;

            _roleManager = roleManager;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
        }

        [AllowAnonymous]
        public async Task<GenerateAccessTokenOutput> GenerateAccessToken(GenerateAccessTokenInput input)
        {
            if (!IsValidEmail(input.Email))
            {
                throw new Exception("Invalid email address!");
            }

            var tenant = await _tenantRepository.GetAll().AsNoTracking().Where(x => x.TenancyName == input.TenancyName).FirstOrDefaultAsync();

            if (tenant.IsNullOrDeleted())
            {
                await CreateTenant(input);

                return await GenerateAccessToken(input);
            }

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete, CustomFilterConsts.IsActive))
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                var user = await _userRepository.FirstOrDefaultAsync(x => x.EmailAddress == input.Email);

                var handler = new JwtSecurityTokenHandler();

                var secret = Encoding.UTF8.GetBytes(_configurationAccessor.Configuration["Authentication:JwtBearer:SecurityKey"]);
                var credentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = await GenerateClaims(user, tenant),
                    Issuer = _configurationAccessor.Configuration["Authentication:JwtBearer:Issuer"],
                    Audience = _configurationAccessor.Configuration["Authentication:JwtBearer:Audience"],
                    Expires = DateTime.MaxValue,
                    SigningCredentials = credentials,
                    TokenType = "AccessToken"
                };

                var token = handler.CreateToken(tokenDescriptor);

                await CurrentUnitOfWork.SaveChangesAsync();
                uow.Complete();

                return new GenerateAccessTokenOutput()
                {
                    AccessToken = handler.WriteToken(token),
                };
            }
        }

        public async Task<string> LoginUser(LoginCustomerUserInput input)
        {
            var tenant = await _tenantRepository.FirstOrDefaultAsync(x => x.TenancyName == input.TenancyName);

            try
            {
                using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete, CustomFilterConsts.IsActive))
                using (CurrentUnitOfWork.SetTenantId(tenant.Id))
                {
                    var user = await _userRepository.FirstOrDefaultAsync(x => x.EmailAddress == input.User.Email);

                    if (input.AccessToken.IsNullOrEmpty())
                    {
                        input.User.Id = user.IsNullOrDeleted() ? CreateUser(input.User, tenant.Id).ToString() : user.Id.ToString();
                    }

                    var token = await GenerateAccessToken(new GenerateAccessTokenInput
                    {
                        TenancyName = tenant.TenancyName,
                        Email = user.EmailAddress,
                        Password = input.User.Password
                    });

                    input.AccessToken = token.AccessToken;

                    await CurrentUnitOfWork.SaveChangesAsync();
                    return input.AccessToken;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, $"LoginCustomerUser: {ex.Message}", ex);
                throw new Exception(ex.Message);
            }
        }

        private async Task CreateTenant(GenerateAccessTokenInput input)
        {
            try
            {
                using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete, CustomFilterConsts.IsActive))
                using (CurrentUnitOfWork.SetTenantId(null))
                {
                    var editionId = (await _editionManager.FindByNameAsync("Standard")).Id;

                    await TenantManager.CreateWithAdminUserAsync(
                        input.TenancyName,
                        input.TenancyName,
                        input.Password ?? "123zxc",
                        input.Email,
                        _configurationAccessor.Configuration["ConnectionStrings:Default"],
                        true,
                        editionId,
                        !input.Password.IsNullOrEmpty(),
                        false,
                        null,
                        false,
                        null,
                        adminName: "admin",
                        adminSurname: "admin"
                    );

                    await CurrentUnitOfWork.SaveChangesAsync();
                    uow.Complete();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, $"GenerateAccessToken: {ex.Message}", ex);
                throw new Exception(ex.Message);
            }
        }

        private async Task<ClaimsIdentity> GenerateClaims(User user, Tenant tenant)
        {
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.AddClaim(new Claim(AbpClaimTypes.TenantId, tenant.Id.ToString()));
            claims.AddClaim(new Claim(AbpClaimTypes.UserId, user.Id.ToString()));

            var securityStamp = await UserManager.GetSecurityStampAsync(user);
            claims.AddClaim(new Claim("AspNet.Identity.SecurityStamp", securityStamp));
            claims.AddClaim(new Claim("token_type", "0"));
            claims.AddClaim(new Claim("user_identifier", $"{user.Id}@{tenant.Id}"));

            return claims;
        }

        private async Task<long> CreateUser(UserInput input, int tenantId)
        {
            try
            {
                var user = new User()
                {
                    TenantId = tenantId,
                    Name = input.Name,
                    Surname = input.Surname,
                    EmailAddress = input.Email,
                    UserName = input.Username,
                    ProfileType = EProfileType.Customer,
                    ShouldChangePasswordOnNextLogin = true,
                };

                using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete, CustomFilterConsts.IsActive))
                using (CurrentUnitOfWork.SetTenantId(tenantId))
                {
                    await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                    foreach (var validator in _passwordValidators)
                    {
                        CheckErrors(await validator.ValidateAsync(UserManager, user, input.Password));
                    }

                    user.Password = _passwordHasher.HashPassword(user, input.Password);

                    CheckErrors(await UserManager.CreateAsync(user));
                    await CurrentUnitOfWork.SaveChangesAsync();

                    user.Roles = new Collection<UserRole>();
                    var role = await _roleManager.GetRoleByNameAsync("User");
                    user.Roles.Add(new UserRole(tenantId, user.Id, role.Id));

                    await _appNotifier.WelcomeToTheApplicationAsync(user);


                    await CurrentUnitOfWork.SaveChangesAsync();
                    uow.Complete();
                    return user.Id;

                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, $"GenerateAccessToken: {ex.Message}", ex);
                throw new Exception(ex.Message);
            }
        }

        private static string RemoveAccents(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString().ToUpper();
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new System.Globalization.IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
