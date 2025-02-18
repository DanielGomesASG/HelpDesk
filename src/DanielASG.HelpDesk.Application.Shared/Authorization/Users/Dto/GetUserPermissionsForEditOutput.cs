﻿using System.Collections.Generic;
using DanielASG.HelpDesk.Authorization.Permissions.Dto;

namespace DanielASG.HelpDesk.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}