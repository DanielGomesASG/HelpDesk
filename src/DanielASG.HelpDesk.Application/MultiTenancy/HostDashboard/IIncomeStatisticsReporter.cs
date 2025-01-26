using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DanielASG.HelpDesk.MultiTenancy.HostDashboard.Dto;

namespace DanielASG.HelpDesk.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}