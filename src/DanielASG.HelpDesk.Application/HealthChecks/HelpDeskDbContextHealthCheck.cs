using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using DanielASG.HelpDesk.EntityFrameworkCore;

namespace DanielASG.HelpDesk.HealthChecks
{
    public class HelpDeskDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public HelpDeskDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("HelpDeskDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("HelpDeskDbContext could not connect to database"));
        }
    }
}
