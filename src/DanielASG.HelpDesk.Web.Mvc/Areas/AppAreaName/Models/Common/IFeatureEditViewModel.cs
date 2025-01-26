using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DanielASG.HelpDesk.Editions.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}