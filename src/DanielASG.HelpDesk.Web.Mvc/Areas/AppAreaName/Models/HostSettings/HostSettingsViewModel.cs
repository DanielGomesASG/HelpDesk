﻿using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using DanielASG.HelpDesk.Configuration.Host.Dto;
using DanielASG.HelpDesk.Editions.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }

        public List<string> EnabledSocialLoginSettings { get; set; } = new List<string>();

        public List<string> GetOpenIdConnectResponseTypes()
        {
            return (Settings.ExternalLoginProviderSettings.OpenIdConnect.ResponseType ?? "").Split(',')
                .Select(x => x.Trim()).ToList();
        }
    }
}