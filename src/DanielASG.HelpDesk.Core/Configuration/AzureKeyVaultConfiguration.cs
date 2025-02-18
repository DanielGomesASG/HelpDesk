﻿using Abp.Extensions;

namespace DanielASG.HelpDesk.Configuration
{
    public class AzureKeyVaultConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public string TenantId { get; set; }
        
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
        
        public bool IsEnabled { get; set; }

        public string KeyVaultName { get; set; }
    }
}