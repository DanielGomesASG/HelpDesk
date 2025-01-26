using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DanielASG.HelpDesk.Web.Controllers;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize]
    public class WelcomeController : HelpDeskControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}