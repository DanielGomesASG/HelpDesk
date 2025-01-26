using Abp.Events.Bus;

namespace DanielASG.HelpDesk.MultiTenancy.Subscription
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}