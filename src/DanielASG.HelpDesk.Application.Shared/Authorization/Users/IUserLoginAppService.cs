using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DanielASG.HelpDesk.Authorization.Users.Dto;

namespace DanielASG.HelpDesk.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<PagedResultDto<UserLoginAttemptDto>> GetUserLoginAttempts(GetLoginAttemptsInput input);
    }
}
