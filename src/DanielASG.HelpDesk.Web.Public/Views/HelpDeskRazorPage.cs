using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace DanielASG.HelpDesk.Web.Public.Views
{
    public abstract class HelpDeskRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected HelpDeskRazorPage()
        {
            LocalizationSourceName = HelpDeskConsts.LocalizationSourceName;
        }
    }
}
