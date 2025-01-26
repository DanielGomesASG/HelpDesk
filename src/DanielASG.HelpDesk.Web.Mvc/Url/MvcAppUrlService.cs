using Abp.MultiTenancy;
using DanielASG.HelpDesk.Url;

namespace DanielASG.HelpDesk.Web.Url
{
    public class MvcAppUrlService : AppUrlServiceBase
    {
        public override string EmailActivationRoute => "Account/EmailConfirmation";

        public override string EmailChangeRequestRoute => "Account/EmailChangeRequest";

        public override string PasswordResetRoute => "Account/ResetPassword";

        public override string TicketRoute => "App/Main/Tickets/Tickets";

        public MvcAppUrlService(
                IWebUrlService webUrlService,
                ITenantCache tenantCache
            ) : base(
                webUrlService,
                tenantCache
            )
        {

        }
    }
}