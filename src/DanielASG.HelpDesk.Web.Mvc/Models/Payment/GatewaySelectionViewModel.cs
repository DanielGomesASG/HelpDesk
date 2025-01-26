using System.Collections.Generic;
using System.Linq;
using DanielASG.HelpDesk.MultiTenancy.Payments;
using DanielASG.HelpDesk.MultiTenancy.Payments.Dto;

namespace DanielASG.HelpDesk.Web.Models.Payment
{
    public class GatewaySelectionViewModel
    {
        public SubscriptionPaymentDto Payment { get; set; }
        
        public List<PaymentGatewayModel> PaymentGateways { get; set; }

        public bool AllowRecurringPaymentOption()
        {
            return Payment.AllowRecurringPayment() && PaymentGateways.Any(gateway => gateway.SupportsRecurringPayments);
        }
    }
}
