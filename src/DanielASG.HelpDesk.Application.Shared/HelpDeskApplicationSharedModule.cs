using Abp.Modules;
using Abp.Reflection.Extensions;

namespace DanielASG.HelpDesk
{
    [DependsOn(typeof(HelpDeskCoreSharedModule))]
    public class HelpDeskApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(HelpDeskApplicationSharedModule).GetAssembly());
        }
    }
}