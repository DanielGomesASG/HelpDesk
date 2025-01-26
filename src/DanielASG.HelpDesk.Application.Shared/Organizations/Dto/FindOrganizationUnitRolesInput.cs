using DanielASG.HelpDesk.Dto;

namespace DanielASG.HelpDesk.Organizations.Dto
{
    public class FindOrganizationUnitRolesInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}