using Abp.AutoMapper;
using DanielASG.HelpDesk.Localization.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Languages
{
    [AutoMapFrom(typeof(GetLanguageForEditOutput))]
    public class CreateOrEditLanguageModalViewModel : GetLanguageForEditOutput
    {
        public bool IsEditMode => Language.Id.HasValue;
    }
}