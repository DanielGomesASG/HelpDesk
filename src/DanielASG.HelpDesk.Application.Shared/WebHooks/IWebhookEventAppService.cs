using System.Threading.Tasks;
using Abp.Webhooks;

namespace DanielASG.HelpDesk.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
