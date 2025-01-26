using DanielASG.HelpDesk.Editions;
using DanielASG.HelpDesk.Editions.Dto;
using DanielASG.HelpDesk.MultiTenancy.Payments;
using DanielASG.HelpDesk.Security;
using DanielASG.HelpDesk.MultiTenancy.Payments.Dto;

namespace DanielASG.HelpDesk.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public int? EditionId { get; set; }

        public EditionSelectDto Edition { get; set; }
        
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
        
        public SubscriptionStartType? SubscriptionStartType { get; set; }
        
        public PaymentPeriodType? PaymentPeriodType { get; set; }
        
        public string SuccessUrl { get; set; }
        
        public string ErrorUrl { get; set; }
    }
}
