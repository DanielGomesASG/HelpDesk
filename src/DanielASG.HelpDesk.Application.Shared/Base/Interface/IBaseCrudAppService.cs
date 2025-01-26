using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DanielASG.HelpDesk.Base.Interface
{
    public interface IBaseCrudAppService<TEntityDto> : IAsyncCrudAppService<TEntityDto>
     where TEntityDto : IEntityDto<int>
    {
        Task<TEntityDto> CreateOrEdit(TEntityDto model);
        Task CreateOrEditRange(List<TEntityDto> items);

        Task<BaseSelectInputDto[]> GetAllForSelectInput(string filterText, int?[] initialId);
        Task<PagedResultDto<TEntityDto>> GetAllFilterAsync(GetAllInput input);

    }
}
