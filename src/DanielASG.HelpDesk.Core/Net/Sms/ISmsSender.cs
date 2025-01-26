using System.Threading.Tasks;

namespace DanielASG.HelpDesk.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}