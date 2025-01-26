using DanielASG.HelpDesk.MultiTenancy.Payments.Dto;
using DanielASG.HelpDesk.MultiTenancy.Payments.Stripe;

namespace DanielASG.HelpDesk.Web.Models.Stripe
{
    public class StripePurchaseViewModel
    {
        public SubscriptionPaymentDto Payment { get; set; }
        
        public decimal Amount { get; set; }

        public bool IsRecurring { get; set; }
        
        public bool IsProrationPayment { get; set; }

        public string SessionId { get; set; }

        public StripePaymentGatewayConfiguration Configuration { get; set; }
    }
}
