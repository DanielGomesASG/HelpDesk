using Abp.Dependency;
using DanielASG.HelpDesk.Configuration;
using DanielASG.HelpDesk.Url;
using DanielASG.HelpDesk.Web.Url;

namespace DanielASG.HelpDesk.Web.Public.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor appConfigurationAccessor) :
            base(appConfigurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:AdminWebSiteRootAddress";
    }
}