using Abp.Application.Services.Dto;
using Abp.Webhooks;
using DanielASG.HelpDesk.WebHooks.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
