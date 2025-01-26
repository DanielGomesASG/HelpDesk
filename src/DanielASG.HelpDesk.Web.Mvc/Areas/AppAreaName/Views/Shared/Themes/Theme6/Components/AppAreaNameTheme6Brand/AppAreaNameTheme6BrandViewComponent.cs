using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Layout;
using DanielASG.HelpDesk.Web.Session;
using DanielASG.HelpDesk.Web.Views;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Views.Shared.Themes.Theme6.Components.AppAreaNameTheme6Brand
{
    public class AppAreaNameTheme6BrandViewComponent : HelpDeskViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppAreaNameTheme6BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync(string skin = "dark-sm")
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync(),
            };

            ViewBag.BrandLogoSkin = skin;

            return View(headerModel);
        }
    }
}
