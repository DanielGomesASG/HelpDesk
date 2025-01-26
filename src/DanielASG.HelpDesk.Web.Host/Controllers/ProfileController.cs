using Abp.AspNetCore.Mvc.Authorization;
using DanielASG.HelpDesk.Authorization.Users.Profile;
using DanielASG.HelpDesk.Graphics;
using DanielASG.HelpDesk.Storage;

namespace DanielASG.HelpDesk.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService,
            IImageValidator imageValidator) :
            base(tempFileCacheManager, profileAppService, imageValidator)
        {
        }
    }
}