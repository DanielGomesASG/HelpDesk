using Abp.Application.Services.Dto;

namespace DanielASG.HelpDesk.Base
{
    public class GetAllInput : PagedAndSortedResultRequestDto
    {
        public string FilterText { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime desc";
            }
        }
    }
}
