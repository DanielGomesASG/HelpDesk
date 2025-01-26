using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace DanielASG.HelpDesk.Localization
{
    public static class HelpDeskLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    HelpDeskConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(HelpDeskLocalizationConfigurer).GetAssembly(),
                        "DanielASG.HelpDesk.Localization.HelpDesk"
                    )
                )
            );
        }
    }
}