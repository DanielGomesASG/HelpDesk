using System.Threading.Tasks;

namespace DanielASG.HelpDesk.Tickets
{
    public interface ITicketEmailer
    {
        Task SendStatusEmail(int tenantId, int ticketId, int oldStatusId, int newStatusId);
        Task SendDefaultMessageEmail(int tenantId, int ticketId, string newMessage);
        Task SendMessageEmail(int tenantId, int ticketId, string newMessage);
    }
}
