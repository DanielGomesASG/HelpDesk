using Abp.Modules;
using Abp.Reflection.Extensions;

namespace DanielASG.HelpDesk
{
    public class HelpDeskClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(HelpDeskClientModule).GetAssembly());
        }
    }
}
