using Abp.Auditing;
using DanielASG.HelpDesk.Configuration.Dto;

namespace DanielASG.HelpDesk.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}