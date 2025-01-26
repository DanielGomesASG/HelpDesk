using System.Threading.Tasks;
using Abp.Application.Services;
using DanielASG.HelpDesk.Sessions.Dto;

namespace DanielASG.HelpDesk.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
