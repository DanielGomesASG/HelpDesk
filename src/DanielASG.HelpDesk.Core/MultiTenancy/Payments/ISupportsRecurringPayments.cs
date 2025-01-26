using Abp.Events.Bus.Handlers;
using DanielASG.HelpDesk.MultiTenancy.Subscription;

namespace DanielASG.HelpDesk.MultiTenancy.Payments
{
    public interface ISupportsRecurringPayments : 
        IEventHandler<RecurringPaymentsDisabledEventData>, 
        IEventHandler<RecurringPaymentsEnabledEventData>,
        IEventHandler<SubscriptionUpdatedEventData>,
        IEventHandler<SubscriptionCancelledEventData>
    {

    }
}
