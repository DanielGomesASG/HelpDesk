﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Editions
{
    public class MoveTenantsToAnotherEditionViewModel
    {
        public int EditionId { get; set; }

        public int TenantCount { get; set; }

        public IReadOnlyList<ComboboxItemDto> EditionItems { get; set; }
    }
}