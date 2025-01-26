using System.Collections.Generic;

namespace DanielASG.HelpDesk.Tickets.Dtos
{
    public class GetTicketDetailsOutput
    {
        public CreateOrEditTicketDto Ticket { get; set; }

        public List<TicketMessageDto> Messages { get; set; }

        public string FilesFileName { get; set; }
    }
}
