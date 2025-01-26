using System.Threading.Tasks;
using Abp.Application.Services;
using DanielASG.HelpDesk.MultiTenancy.Payments.Dto;
using DanielASG.HelpDesk.MultiTenancy.Payments.Stripe.Dto;

namespace DanielASG.HelpDesk.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();
        
        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}