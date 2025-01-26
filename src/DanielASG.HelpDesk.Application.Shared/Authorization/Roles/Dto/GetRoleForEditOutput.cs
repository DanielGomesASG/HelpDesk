using System.Collections.Generic;
using DanielASG.HelpDesk.Authorization.Permissions.Dto;

namespace DanielASG.HelpDesk.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}