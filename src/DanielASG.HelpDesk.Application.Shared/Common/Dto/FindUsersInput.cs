using DanielASG.HelpDesk.Dto;

namespace DanielASG.HelpDesk.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }

        public bool ExcludeCurrentUser { get; set; }
    }
}