using System;

namespace DanielASG.HelpDesk.Notifications.Dto
{
    public class GetPublishedNotificationsInput
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}