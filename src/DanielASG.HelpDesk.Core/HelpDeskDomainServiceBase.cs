using Abp.Domain.Services;

namespace DanielASG.HelpDesk
{
    public abstract class HelpDeskDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected HelpDeskDomainServiceBase()
        {
            LocalizationSourceName = HelpDeskConsts.LocalizationSourceName;
        }
    }
}
