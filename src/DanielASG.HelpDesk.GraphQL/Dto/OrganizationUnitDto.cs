using Abp.AutoMapper;
using Abp.Organizations;

namespace DanielASG.HelpDesk.Dto
{
    [AutoMapFrom(typeof(OrganizationUnit))]
    public class OrganizationUnitDto
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string DisplayName { get; set; }

        public int? TenantId { get; set; }
    }
}