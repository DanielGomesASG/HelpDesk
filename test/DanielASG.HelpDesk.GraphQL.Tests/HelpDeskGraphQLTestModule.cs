using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using DanielASG.HelpDesk.Configure;
using DanielASG.HelpDesk.Startup;
using DanielASG.HelpDesk.Test.Base;

namespace DanielASG.HelpDesk.GraphQL.Tests
{
    [DependsOn(
        typeof(HelpDeskGraphQLModule),
        typeof(HelpDeskTestBaseModule))]
    public class HelpDeskGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(HelpDeskGraphQLTestModule).GetAssembly());
        }
    }
}