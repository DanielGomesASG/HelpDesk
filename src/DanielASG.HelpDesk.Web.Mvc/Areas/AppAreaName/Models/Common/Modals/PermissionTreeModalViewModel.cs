using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Authorization.Permissions.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Common.Modals
{
    public class PermissionTreeModalViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }
        public List<string> GrantedPermissionNames { get; set; }
    }
}
