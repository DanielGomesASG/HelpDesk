using Abp.AutoMapper;
using DanielASG.HelpDesk.MultiTenancy;
using DanielASG.HelpDesk.MultiTenancy.Dto;
using DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Common;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}