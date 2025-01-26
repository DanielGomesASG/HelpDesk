using Abp.AspNetCore.Mvc.ViewComponents;

namespace DanielASG.HelpDesk.Web.Public.Views
{
    public abstract class HelpDeskViewComponent : AbpViewComponent
    {
        protected HelpDeskViewComponent()
        {
            LocalizationSourceName = HelpDeskConsts.LocalizationSourceName;
        }
    }
}