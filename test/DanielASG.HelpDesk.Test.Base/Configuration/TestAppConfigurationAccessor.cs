using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using DanielASG.HelpDesk.Configuration;

namespace DanielASG.HelpDesk.Test.Base.Configuration
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(HelpDeskTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
