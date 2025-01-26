using System.Collections.Generic;
using DanielASG.HelpDesk.Organizations.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Common
{
    public interface IOrganizationUnitsEditViewModel
    {
        List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        List<string> MemberedOrganizationUnits { get; set; }
    }
}