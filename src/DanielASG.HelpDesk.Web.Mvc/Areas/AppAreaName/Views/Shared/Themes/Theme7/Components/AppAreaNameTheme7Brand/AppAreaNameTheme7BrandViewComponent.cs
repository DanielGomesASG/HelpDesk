using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Layout;
using DanielASG.HelpDesk.Web.Session;
using DanielASG.HelpDesk.Web.Views;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Views.Shared.Themes.Theme7.Components.AppAreaNameTheme7Brand
{
    public class AppAreaNameTheme7BrandViewComponent : HelpDeskViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppAreaNameTheme7BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync(),
            };

            return View(headerModel);
        }
    }
}
