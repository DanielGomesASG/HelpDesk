using Abp.MultiTenancy;
using DanielASG.HelpDesk.Url;

namespace DanielASG.HelpDesk.Web.Url
{
    public class AngularAppUrlService : AppUrlServiceBase
    {
        public override string EmailActivationRoute => "account/confirm-email";

        public override string EmailChangeRequestRoute => "account/change-email";

        public override string PasswordResetRoute => "account/reset-password";

        public override string TicketRoute => "app/main/tickets/tickets";

        public AngularAppUrlService(
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