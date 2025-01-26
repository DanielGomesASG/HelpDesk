using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DanielASG.HelpDesk.Authorization.Permissions.Dto;

namespace DanielASG.HelpDesk.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
