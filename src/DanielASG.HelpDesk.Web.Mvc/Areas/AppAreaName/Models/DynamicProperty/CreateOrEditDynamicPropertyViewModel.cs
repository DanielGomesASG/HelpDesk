using System.Collections.Generic;
using DanielASG.HelpDesk.DynamicEntityProperties.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.DynamicProperty
{
    public class CreateOrEditDynamicPropertyViewModel
    {
        public DynamicPropertyDto DynamicPropertyDto { get; set; }

        public List<string> AllowedInputTypes { get; set; }
    }
}
