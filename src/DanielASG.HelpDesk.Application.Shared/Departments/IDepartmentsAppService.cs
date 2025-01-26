using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Base;
using DanielASG.HelpDesk.Departments.Dtos;

namespace DanielASG.HelpDesk.Departments
{
    public interface IDepartmentsAppService : IApplicationService
    {
        Task<PagedResultDto<DepartmentDto>> GetAll(GetAllDepartmentsInput input);

        Task<CreateOrEditDepartmentDto> GetDepartmentForEdit(EntityDto input);

        Task<BaseSelectInputDto[]> GetAllForSelectInput(string filterText, int?[] initialId);

        Task CreateOrEdit(CreateOrEditDepartmentDto input);

        Task Delete(EntityDto input);

    }
}