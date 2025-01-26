using System.Threading.Tasks;
using DanielASG.HelpDesk.Sessions.Dto;

namespace DanielASG.HelpDesk.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
