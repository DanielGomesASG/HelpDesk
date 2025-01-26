using System.Collections.Generic;
using DanielASG.HelpDesk.DynamicEntityProperties.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.DynamicEntityProperty
{
    public class CreateEntityDynamicPropertyViewModel
    {
        public string EntityFullName { get; set; }

        public List<string> AllEntities { get; set; }

        public List<DynamicPropertyDto> DynamicProperties { get; set; }
    }
}
