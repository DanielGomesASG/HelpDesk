﻿using Microsoft.AspNetCore.Antiforgery;

namespace DanielASG.HelpDesk.Web.Controllers
{
    public class AntiForgeryController : HelpDeskControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
