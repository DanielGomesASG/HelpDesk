using DanielASG.HelpDesk.MultiTenancy.Dto;
using DanielASG.HelpDesk.Sessions.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Editions
{
    public class SubscriptionDashboardViewModel
    {
        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }
        
        public EditionsSelectOutput Editions { get; set; }
    }
}
