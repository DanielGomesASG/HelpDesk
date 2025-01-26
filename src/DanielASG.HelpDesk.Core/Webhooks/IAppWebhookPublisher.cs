using System.Threading.Tasks;
using DanielASG.HelpDesk.Authorization.Users;

namespace DanielASG.HelpDesk.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
