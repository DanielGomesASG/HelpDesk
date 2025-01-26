using System.Collections.Generic;
using Abp.Notifications;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Notifications
{
    public class CreateMassNotificationViewModel
    {
        public List<string> TargetNotifiers { get; set; }
    
        public NotificationSeverity Severity { get; set; }
    }
}