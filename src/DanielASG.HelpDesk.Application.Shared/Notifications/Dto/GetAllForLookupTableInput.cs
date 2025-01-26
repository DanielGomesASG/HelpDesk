using Abp.Application.Services.Dto;

namespace DanielASG.HelpDesk.Notifications.Dto
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}