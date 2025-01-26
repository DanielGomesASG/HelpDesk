using DanielASG.HelpDesk.Dto;

namespace DanielASG.HelpDesk.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
