using Abp.AspNetZeroCore;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using DanielASG.HelpDesk.Auditing;
using DanielASG.HelpDesk.Authorization.Users.Password;
using DanielASG.HelpDesk.Configuration;
using DanielASG.HelpDesk.EntityFrameworkCore;
using DanielASG.HelpDesk.MultiTenancy.Subscription;
using DanielASG.HelpDesk.Tickets.Worker;
using DanielASG.HelpDesk.Web.Areas.AppAreaName.Startup;

namespace DanielASG.HelpDesk.Web.Startup
{
    [DependsOn(
        typeof(HelpDeskWebCoreModule)
    )]
    public class HelpDeskWebMvcModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public HelpDeskWebMvcModule(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat = _appConfiguration["App:WebSiteRootAddress"] ?? "https://localhost:44302/";
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];
            Configuration.Navigation.Providers.Add<AppAreaNameNavigationProvider>();

            IocManager.Register<DashboardViewConfiguration>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(HelpDeskWebMvcModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!IocManager.Resolve<IMultiTenancyConfig>().IsEnabled)
            {
                return;
            }

            using (var scope = IocManager.CreateScope())
            {
                if (!scope.Resolve<DatabaseCheckHelper>().Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    return;
                }
            }

            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<SubscriptionExpirationCheckWorker>());
            workManager.Add(IocManager.Resolve<SubscriptionExpireEmailNotifierWorker>());
            workManager.Add(IocManager.Resolve<SubscriptionPaymentNotCompletedEmailNotifierWorker>());

            var expiredAuditLogDeleterWorker = IocManager.Resolve<ExpiredAuditLogDeleterWorker>();
            if (Configuration.Auditing.IsEnabled && expiredAuditLogDeleterWorker.IsEnabled)
            {
                workManager.Add(expiredAuditLogDeleterWorker);
            }

            workManager.Add(IocManager.Resolve<PasswordExpirationBackgroundWorker>());

            var ticketEmailWorker = IocManager.Resolve<TicketEmailWorker>();
            if (ticketEmailWorker.IsEnabled)
            {
                workManager.Add(ticketEmailWorker);
            }
        }
    }
}