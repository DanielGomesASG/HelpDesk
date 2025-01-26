using Abp.AutoMapper;
using DanielASG.HelpDesk.MultiTenancy.Dto;

namespace DanielASG.HelpDesk.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}