using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Layout;
using DanielASG.HelpDesk.Web.Session;
using DanielASG.HelpDesk.Web.Views;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Views.Shared.Themes.Theme2.Components.AppAreaNameTheme2Brand
{
    public class AppAreaNameTheme2BrandViewComponent : HelpDeskViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppAreaNameTheme2BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
