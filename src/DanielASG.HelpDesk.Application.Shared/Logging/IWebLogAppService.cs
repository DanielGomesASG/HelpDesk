using Abp.Application.Services;
using DanielASG.HelpDesk.Dto;
using DanielASG.HelpDesk.Logging.Dto;

namespace DanielASG.HelpDesk.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
