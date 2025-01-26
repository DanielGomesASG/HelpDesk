using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DanielASG.HelpDesk.Caching.Dto;

namespace DanielASG.HelpDesk.Caching
{
    public interface ICachingAppService : IApplicationService
    {
        ListResultDto<CacheDto> GetAllCaches();

        Task ClearCache(EntityDto<string> input);

        Task ClearAllCaches();
        
        bool CanClearAllCaches();
    }
}
