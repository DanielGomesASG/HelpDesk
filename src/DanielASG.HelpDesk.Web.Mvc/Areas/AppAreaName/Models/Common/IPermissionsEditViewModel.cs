using System.Collections.Generic;
using DanielASG.HelpDesk.Authorization.Permissions.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}