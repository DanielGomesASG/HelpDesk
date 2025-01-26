using System.Collections.Generic;
using DanielASG.HelpDesk.DashboardCustomization.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.CustomizableDashboard
{
    public class AddWidgetViewModel
    {
        public List<WidgetOutput> Widgets { get; set; }

        public string DashboardName { get; set; }

        public string PageId { get; set; }
    }
}
