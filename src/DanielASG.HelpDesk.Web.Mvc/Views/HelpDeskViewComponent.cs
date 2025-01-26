using Abp.AspNetCore.Mvc.ViewComponents;

namespace DanielASG.HelpDesk.Web.Views
{
    public abstract class HelpDeskViewComponent : AbpViewComponent
    {
        protected HelpDeskViewComponent()
        {
            LocalizationSourceName = HelpDeskConsts.LocalizationSourceName;
        }
    }
}