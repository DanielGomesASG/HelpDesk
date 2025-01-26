using DanielASG.HelpDesk.DashboardCustomization;
using DanielASG.HelpDesk.DashboardCustomization.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.CustomizableDashboard
{
    public class CustomizableDashboardViewModel
    {
        public DashboardOutput DashboardOutput { get; }

        public Dashboard UserDashboard { get; }

        public CustomizableDashboardViewModel(
            DashboardOutput dashboardOutput,
            Dashboard userDashboard)
        {
            DashboardOutput = dashboardOutput;
            UserDashboard = userDashboard;
        }
    }
}