using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DanielASG.HelpDesk.Common.Dto;
using DanielASG.HelpDesk.Editions.Dto;

namespace DanielASG.HelpDesk.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<FindUsersOutputDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();
    }
}