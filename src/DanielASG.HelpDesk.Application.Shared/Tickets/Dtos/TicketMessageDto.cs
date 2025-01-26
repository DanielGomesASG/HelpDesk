using Abp.Application.Services.Dto;
using System;

namespace DanielASG.HelpDesk.Tickets.Dtos
{
    public class TicketMessageDto : EntityDto
    {
        public string Message { get; set; }

        public int TicketId { get; set; }

        public int CreatorUserId { get; set; }

        public DateTime? CreationTime { get; set; }
    }
}
