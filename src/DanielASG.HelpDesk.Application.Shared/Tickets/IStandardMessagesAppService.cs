using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Base;
using DanielASG.HelpDesk.Tickets.Dtos;

namespace DanielASG.HelpDesk.Tickets
{
    public interface IStandardMessagesAppService : IApplicationService
    {
        Task<PagedResultDto<StandardMessageDto>> GetAll(GetAllStandardMessagesInput input);

        Task<CreateOrEditStandardMessageDto> GetStandardMessageForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditStandardMessageDto input);

        Task Delete(EntityDto input);

        Task<BaseSelectInputDto[]> GetAllForSelectInput(string filterText, long?[] initialId, int? messageTypeId);

    }
}