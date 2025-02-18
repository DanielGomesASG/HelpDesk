﻿using System.Collections.Generic;
using System.Linq;
using Abp.Authorization;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using DanielASG.HelpDesk.Authorization;
using DanielASG.HelpDesk.DashboardCustomization.Definitions.Cache;

namespace DanielASG.HelpDesk.DashboardCustomization.Definitions
{
    public class DashboardConfiguration : ITransientDependency
    {
        public string TenantWidgetDefinitionsCacheName = "TenantWidgetDefinitionsCache";
        public const string HostWidgetDefinitionsCacheName = "HostWidgetDefinitionsCache";

        private readonly IDashboardDefinitionCacheManager _dashboardDefinitionCacheManager;
        private readonly IWidgetDefinitionCacheManager _widgetDefinitionCacheManager;
        private readonly IWidgetFilterDefinitionCacheManager _widgetFilterDefinitionCacheManager;

        private readonly IAbpSession _abpSession;

        private List<DashboardDefinition> DashboardDefinitions { get; } = new();
        private List<WidgetDefinition> WidgetDefinitions { get; } = new();
        private List<WidgetFilterDefinition> WidgetFilterDefinitions { get; } = new();

        public DashboardConfiguration(
            IDashboardDefinitionCacheManager dashboardDefinitionCacheManager,
            IWidgetDefinitionCacheManager widgetDefinitionCacheManager,
            IWidgetFilterDefinitionCacheManager widgetFilterDefinitionCacheManager,
            IAbpSession abpSession)
        {
            _dashboardDefinitionCacheManager = dashboardDefinitionCacheManager;
            _widgetDefinitionCacheManager = widgetDefinitionCacheManager;
            _widgetFilterDefinitionCacheManager = widgetFilterDefinitionCacheManager;
            _abpSession = abpSession;

            #region FilterDefinitions

            // These are global filter which all widgets can use
            var dateRangeFilter = new WidgetFilterDefinition(
                HelpDeskDashboardCustomizationConsts.Filters.FilterDateRangePicker,
                "FilterDateRangePicker"
            );

            WidgetFilterDefinitions.AddRange(new List<WidgetFilterDefinition>()
            {
                dateRangeFilter
                // Add your filters here
            });

            #endregion

            #region WidgetDefinitions

            // Define Widgets

            #region TenantWidgets

            var simplePermissionDependencyForTenantDashboard =
                new HelpDeskSimplePermissionDependency(AppPermissions.Pages_Tenant_Dashboard);

            var dailySales = new WidgetDefinition(
                HelpDeskDashboardCustomizationConsts.Widgets.Tenant.DailySales,
                "WidgetDailySales",
                side: MultiTenancySides.Tenant,
                usedWidgetFilters: new List<string> {dateRangeFilter.Id},
                permissionDependency: simplePermissionDependencyForTenantDashboard
            );

            var generalStats = new WidgetDefinition(
                HelpDeskDashboardCustomizationConsts.Widgets.Tenant.GeneralStats,
                "WidgetGeneralStats",
                side: MultiTenancySides.Tenant,
                permissionDependency: new HelpDeskSimplePermissionDependency(
                    requiresAll: true,
                    AppPermissions.Pages_Tenant_Dashboard,
                    AppPermissions.Pages_Administration_AuditLogs
                )
            );

            var profitShare = new WidgetDefinition(
                HelpDeskDashboardCustomizationConsts.Widgets.Tenant.ProfitShare,
                "WidgetProfitShare",
                side: MultiTenancySides.Tenant,
                permissionDependency: simplePermissionDependencyForTenantDashboard
            );

            var memberActivity = new WidgetDefinition(
                HelpDeskDashboardCustomizationConsts.Widgets.Tenant.MemberActivity,
                "WidgetMemberActivity",
                side: MultiTenancySides.Tenant,
                permissionDependency: simplePermissionDependencyForTenantDashboard
            );

            var regionalStats = new WidgetDefinition(
                HelpDeskDashboardCustomizationConsts.Widgets.Tenant.RegionalStats,
                "WidgetRegionalStats",
                side: MultiTenancySides.Tenant,
                permissionDependency: simplePermissionDependencyForTenantDashboard
            );

            var salesSummary = new WidgetDefinition(
                HelpDeskDashboardCustomizationConsts.Widgets.Tenant.SalesSummary,
                "WidgetSalesSummary",
                usedWidgetFilters: new List<string>() {dateRangeFilter.Id},
                side: MultiTenancySides.Tenant,
                permissionDependency: simplePermissionDependencyForTenantDashboard
            );

            var topStats = new WidgetDefinition(
                HelpDeskDashboardCustomizationConsts.Widgets.Tenant.TopStats,
                "WidgetTopStats",
                side: MultiTenancySides.Tenant,
                permissionDependency: simplePermissionDependencyForTenantDashboard
            );

            WidgetDefinitions.AddRange(
                new List<WidgetDefinition>
                {
                    generalStats,
                    dailySales,
                    profitShare,
                    memberActivity,
                    regionalStats,
                    topStats,
                    salesSummary
                    // Add your tenant side widgets here
                });

            #endregion

            #region HostWidgets

            var simplePermissionDependencyForHostDashboard =
                new HelpDeskSimplePermissionDependency(AppPermissions.Pages_Administration_Host_Dashboard);

            var incomeStatistics = new WidgetDefinition(
                HelpDeskDashboardCustomizationConsts.Widgets.Host.IncomeStatistics,
                "WidgetIncomeStatistics",
                side: MultiTenancySides.Host,
                permissionDependency: simplePermissionDependencyForHostDashboard
            );

            var hostTopStats = new WidgetDefinition(
                HelpDeskDashboardCustomizationConsts.Widgets.Host.TopStats,
                "WidgetTopStats",
                side: MultiTenancySides.Host,
                permissionDependency: simplePermissionDependencyForHostDashboard
            );

            var editionStatistics = new WidgetDefinition(
                HelpDeskDashboardCustomizationConsts.Widgets.Host.EditionStatistics,
                "WidgetEditionStatistics",
                side: MultiTenancySides.Host,
                permissionDependency: simplePermissionDependencyForHostDashboard
            );

            var subscriptionExpiringTenants = new WidgetDefinition(
                HelpDeskDashboardCustomizationConsts.Widgets.Host.SubscriptionExpiringTenants,
                "WidgetSubscriptionExpiringTenants",
                side: MultiTenancySides.Host,
                permissionDependency: simplePermissionDependencyForHostDashboard
            );

            var recentTenants = new WidgetDefinition(
                HelpDeskDashboardCustomizationConsts.Widgets.Host.RecentTenants,
                "WidgetRecentTenants",
                side: MultiTenancySides.Host,
                usedWidgetFilters: new List<string>() {dateRangeFilter.Id},
                permissionDependency: simplePermissionDependencyForHostDashboard
            );

            WidgetDefinitions.AddRange(new List<WidgetDefinition>()
            {
                incomeStatistics,
                hostTopStats,
                editionStatistics,
                subscriptionExpiringTenants,
                recentTenants
                // Add your host side widgets here
            });

            #endregion

            #endregion

            #region DashboardDefinitions

            // Create dashboard
            var defaultTenantDashboard = new DashboardDefinition(
                HelpDeskDashboardCustomizationConsts.DashboardNames.DefaultTenantDashboard,
                new List<string>
                {
                    generalStats.Id, dailySales.Id, profitShare.Id, memberActivity.Id, regionalStats.Id, topStats.Id,
                    salesSummary.Id
                });

            DashboardDefinitions.Add(defaultTenantDashboard);

            var defaultHostDashboard = new DashboardDefinition(
                HelpDeskDashboardCustomizationConsts.DashboardNames.DefaultHostDashboard,
                new List<string>
                {
                    incomeStatistics.Id,
                    hostTopStats.Id,
                    editionStatistics.Id,
                    subscriptionExpiringTenants.Id,
                    recentTenants.Id
                });

            DashboardDefinitions.Add(defaultHostDashboard);

            // Add your dashboard definition here

            #endregion
        }

        public DashboardDefinition GetDashboardDefinition(string name)
        {
            var dashboardDefinition = _dashboardDefinitionCacheManager.Get(name);
            if (dashboardDefinition == null)
            {
                dashboardDefinition = DashboardDefinitions.Find(d => d.Name == name);
                _dashboardDefinitionCacheManager.Set(dashboardDefinition);
            }

            return dashboardDefinition;
        }

        public WidgetDefinition GetWidgetDefinition(string id)
        {
            var widgets = GetWidgetDefinitions();
            return widgets.Find(w => w.Id == id);
        }

        public List<WidgetDefinition> GetWidgetDefinitions()
        {
            var widgetDefinitionKey = _abpSession.MultiTenancySide == MultiTenancySides.Host
                ? HostWidgetDefinitionsCacheName
                : TenantWidgetDefinitionsCacheName;

            var widgetDefinitions = _widgetDefinitionCacheManager.GetAll(widgetDefinitionKey);
            if (widgetDefinitions == null)
            {
                widgetDefinitions = WidgetDefinitions.Where(e => e.Side == _abpSession.MultiTenancySide).ToList();
                _widgetDefinitionCacheManager.Set(widgetDefinitionKey, widgetDefinitions);
            }

            return widgetDefinitions;
        }

        public List<WidgetFilterDefinition> GetWidgetFilterDefinitions()
        {
            var filterDefinitions =  _widgetFilterDefinitionCacheManager.GetAll();
            if (filterDefinitions == null)
            {
                filterDefinitions = WidgetFilterDefinitions;
                _widgetFilterDefinitionCacheManager.Set(filterDefinitions);
            }

            return filterDefinitions;
        }
    }
}