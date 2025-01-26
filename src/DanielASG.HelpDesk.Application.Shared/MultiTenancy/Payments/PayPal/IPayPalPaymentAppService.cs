using System.Threading.Tasks;
using Abp.Application.Services;
using DanielASG.HelpDesk.MultiTenancy.Payments.PayPal.Dto;

namespace DanielASG.HelpDesk.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
