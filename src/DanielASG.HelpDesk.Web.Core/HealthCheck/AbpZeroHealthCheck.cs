﻿using Microsoft.Extensions.DependencyInjection;
using DanielASG.HelpDesk.HealthChecks;

namespace DanielASG.HelpDesk.Web.HealthCheck
{
    public static class AbpZeroHealthCheck
    {
        public static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<HelpDeskDbContextHealthCheck>("Database Connection");
            builder.AddCheck<HelpDeskDbContextUsersHealthCheck>("Database Connection with user check");
            builder.AddCheck<CacheHealthCheck>("Cache");

            // add your custom health checks here
            // builder.AddCheck<MyCustomHealthCheck>("my health check");

            return builder;
        }
    }
}
