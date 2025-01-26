using DanielASG.HelpDesk.Dto;

namespace DanielASG.HelpDesk.Organizations.Dto
{
    public class FindOrganizationUnitUsersInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}
