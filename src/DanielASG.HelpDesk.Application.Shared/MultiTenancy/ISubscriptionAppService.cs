using System.Threading.Tasks;
using Abp.Application.Services;
using DanielASG.HelpDesk.MultiTenancy.Dto;
using DanielASG.HelpDesk.MultiTenancy.Payments.Dto;

namespace DanielASG.HelpDesk.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
        
        Task<long> StartExtendSubscription(StartExtendSubscriptionInput input);
        
        Task<StartUpgradeSubscriptionOutput> StartUpgradeSubscription(StartUpgradeSubscriptionInput input);
        
        Task<long> StartTrialToBuySubscription(StartTrialToBuySubscriptionInput input);
    }
}
