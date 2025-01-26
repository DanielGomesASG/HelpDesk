using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Authorization.Users.Dto;
using DanielASG.HelpDesk.Base;
using DanielASG.HelpDesk.Dto;

namespace DanielASG.HelpDesk.Authorization.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input);

        Task<BaseSelectInputDto[]> GetAllForSelectInput(string filterText, long?[] initialId);

        Task<FileDto> GetUsersToExcel(GetUsersToExcelInput input);

        Task<List<string>> GetUserExcelColumnsToExcel();

        Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input);

        Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input);

        Task ResetUserSpecificPermissions(EntityDto<long> input);

        Task UpdateUserPermissions(UpdateUserPermissionsInput input);

        Task CreateOrUpdateUser(CreateOrUpdateUserInput input);

        Task DeleteUser(EntityDto<long> input);

        Task UnlockUser(EntityDto<long> input);

        Task<string> GeneratePasswordlessTokenAsync(long userId);
    }
}