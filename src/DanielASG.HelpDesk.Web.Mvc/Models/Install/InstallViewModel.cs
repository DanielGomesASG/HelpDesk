using System.Collections.Generic;
using Abp.Localization;
using DanielASG.HelpDesk.Install.Dto;

namespace DanielASG.HelpDesk.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
