using Abp.AspNetCore.Mvc.Views;

namespace DanielASG.HelpDesk.Web.Views
{
    public abstract class HelpDeskRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected HelpDeskRazorPage()
        {
            LocalizationSourceName = HelpDeskConsts.LocalizationSourceName;
        }
    }
}
