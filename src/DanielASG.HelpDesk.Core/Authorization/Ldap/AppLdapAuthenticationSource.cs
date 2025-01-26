using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using DanielASG.HelpDesk.Authorization.Users;
using DanielASG.HelpDesk.MultiTenancy;

namespace DanielASG.HelpDesk.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}