using Microsoft.AspNetCore.Mvc;
using DanielASG.HelpDesk.Web.Controllers;

namespace DanielASG.HelpDesk.Web.Public.Controllers
{
    public class AboutController : HelpDeskControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}