using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace DanielASG.HelpDesk.Startup
{
    [DependsOn(typeof(HelpDeskCoreModule))]
    public class HelpDeskGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(HelpDeskGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}