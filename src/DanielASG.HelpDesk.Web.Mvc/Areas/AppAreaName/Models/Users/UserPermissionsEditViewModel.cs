using Abp.AutoMapper;
using DanielASG.HelpDesk.Authorization.Users;
using DanielASG.HelpDesk.Authorization.Users.Dto;
using DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Common;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; set; }
    }
}