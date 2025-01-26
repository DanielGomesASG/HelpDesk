using System.Collections.Generic;

namespace DanielASG.HelpDesk.MultiTenancy.Payments
{
    public interface IPaymentGatewayStore
    {
        List<PaymentGatewayModel> GetActiveGateways();
    }
}
