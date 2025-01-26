using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using DanielASG.HelpDesk.Authentication.TwoFactor.Google;
using DanielASG.HelpDesk.Authorization;
using DanielASG.HelpDesk.Authorization.Roles;
using DanielASG.HelpDesk.Authorization.Users;
using DanielASG.HelpDesk.Editions;
using DanielASG.HelpDesk.MultiTenancy;

namespace DanielASG.HelpDesk.Identity
{
    public static class IdentityRegistrar
    {
        public static IdentityBuilder Register(IServiceCollection services)
        {
            services.AddLogging();

            return services.AddAbpIdentity<Tenant, User, Role>(options =>
                {
                    options.Tokens.ProviderMap[GoogleAuthenticatorProvider.Name] = new TokenProviderDescriptor(typeof(GoogleAuthenticatorProvider));
                })
                .AddAbpTenantManager<TenantManager>()
                .AddAbpUserManager<UserManager>()
                .AddAbpRoleManager<RoleManager>()
                .AddAbpEditionManager<EditionManager>()
                .AddAbpUserStore<UserStore>()
                .AddAbpRoleStore<RoleStore>()
                .AddAbpSignInManager<SignInManager>()
                .AddAbpUserClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
                .AddAbpSecurityStampValidator<SecurityStampValidator>()
                .AddPermissionChecker<PermissionChecker>()
                .AddDefaultTokenProviders();
        }
    }
}
