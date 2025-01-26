using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Tickets.Dtos;

namespace DanielASG.HelpDesk.Tickets
{
    public interface ITicketsAppService : IApplicationService
    {
        Task<PagedResultDto<TicketDto>> GetAll(GetAllTicketsInput input);

        Task<GetTicketDetailsOutput> GetTicketDetails(EntityDto input);

        Task<GetTicketForEditOutput> GetTicketForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTicketDto input);

        Task InsertTicketMessage(CreateOrEditTicketDto input);

        Task Bind(CreateOrEditTicketDto input);

        Task Finish(CreateOrEditTicketDto input);

        Task Delete(EntityDto input);

        Task RemoveFilesFile(EntityDto input);

    }
}