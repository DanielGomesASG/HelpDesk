using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DanielASG.HelpDesk.Editions.Dto;

namespace DanielASG.HelpDesk.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}