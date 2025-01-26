using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Layout;
using DanielASG.HelpDesk.Web.Views;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Views.Shared.Components.AppAreaNameToggleDarkMode
{
    public class AppAreaNameToggleDarkModeViewComponent : HelpDeskViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass, bool isDarkModeActive)
        {
            return Task.FromResult<IViewComponentResult>(View(new ToggleDarkModeViewModel(cssClass, isDarkModeActive)));
        }
    }
}