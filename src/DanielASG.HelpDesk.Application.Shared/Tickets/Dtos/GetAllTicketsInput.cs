using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace DanielASG.HelpDesk.Tickets.Dtos
{
    public class GetAllTicketsInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public int? UserId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "T.Id DESC";
            }
        }
    }
}