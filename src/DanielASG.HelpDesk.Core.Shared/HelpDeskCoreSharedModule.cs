using Abp.Modules;
using Abp.Reflection.Extensions;

namespace DanielASG.HelpDesk
{
    public class HelpDeskCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(HelpDeskCoreSharedModule).GetAssembly());
        }
    }
}