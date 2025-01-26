using System;
using Abp.Notifications;
using DanielASG.HelpDesk.Dto;

namespace DanielASG.HelpDesk.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}