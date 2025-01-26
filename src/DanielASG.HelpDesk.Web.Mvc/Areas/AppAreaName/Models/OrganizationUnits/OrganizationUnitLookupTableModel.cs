using System.Collections.Generic;
using DanielASG.HelpDesk.Organizations.Dto;
using DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Common;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.OrganizationUnits
{
    public class OrganizationUnitLookupTableModel : IOrganizationUnitsEditViewModel
    {
        public List<OrganizationUnitDto> AllOrganizationUnits { get; set; }
        
        public List<string> MemberedOrganizationUnits { get; set; }

        public OrganizationUnitLookupTableModel()
        {
            AllOrganizationUnits = new List<OrganizationUnitDto>();
            MemberedOrganizationUnits = new List<string>();
        }
    }
}