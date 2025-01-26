using Abp.AutoMapper;
using DanielASG.HelpDesk.Sessions.Dto;

namespace DanielASG.HelpDesk.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}