using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace DanielASG.HelpDesk.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
