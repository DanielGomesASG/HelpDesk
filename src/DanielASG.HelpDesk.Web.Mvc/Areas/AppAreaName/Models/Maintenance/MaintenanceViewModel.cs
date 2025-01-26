using System.Collections.Generic;
using DanielASG.HelpDesk.Caching.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
        
        public bool CanClearAllCaches { get; set; }
    }
}