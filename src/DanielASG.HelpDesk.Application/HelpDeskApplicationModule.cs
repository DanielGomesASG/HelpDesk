using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using DanielASG.HelpDesk.Authorization;

namespace DanielASG.HelpDesk
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(HelpDeskApplicationSharedModule),
        typeof(HelpDeskCoreModule)
        )]
    public class HelpDeskApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(HelpDeskApplicationModule).GetAssembly());
        }
    }
}