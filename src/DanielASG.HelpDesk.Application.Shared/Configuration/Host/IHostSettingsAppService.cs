using System.Threading.Tasks;
using Abp.Application.Services;
using DanielASG.HelpDesk.Configuration.Host.Dto;

namespace DanielASG.HelpDesk.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
