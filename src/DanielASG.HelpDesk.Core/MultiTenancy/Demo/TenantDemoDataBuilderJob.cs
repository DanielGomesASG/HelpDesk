using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using System.Threading.Tasks;
using DanielASG.HelpDesk.MultiTenancy.Base;

namespace DanielASG.HelpDesk.MultiTenancy.Demo
{
    public class TenantDemoDataBuilderJob : AsyncBackgroundJob<int>, ITransientDependency
    {
        private readonly TenantDemoDataBuilder _tenantDemoDataBuilder;
        private readonly TenantBaseDataBuilder _tenantBaseDataBuilder;
        private readonly TenantManager _tenantManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TenantDemoDataBuilderJob(
            TenantDemoDataBuilder tenantDemoDataBuilder,
            TenantBaseDataBuilder tenantBaseDataBuilder,
            TenantManager tenantManager,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _tenantDemoDataBuilder = tenantDemoDataBuilder;
            _tenantBaseDataBuilder = tenantBaseDataBuilder;
            _tenantManager = tenantManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override async Task ExecuteAsync(int args)
        {
            var tenantId = args;
            var tenant = await _tenantManager.GetByIdAsync(tenantId);
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _tenantBaseDataBuilder.BuildForAsync(tenant);
                await _tenantDemoDataBuilder.BuildForAsync(tenant);
                await uow.CompleteAsync();
            }
        }
    }
}
