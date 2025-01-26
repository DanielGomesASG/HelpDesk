using Abp.Application.Services;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Integration.Dtos;

namespace DanielASG.HelpDesk.Integration
{
    public interface IIntegrationAppService : IApplicationService
    {
        Task<GenerateAccessTokenOutput> GenerateAccessToken(GenerateAccessTokenInput input);

        Task<string> LoginUser(LoginCustomerUserInput input);
    }
}
