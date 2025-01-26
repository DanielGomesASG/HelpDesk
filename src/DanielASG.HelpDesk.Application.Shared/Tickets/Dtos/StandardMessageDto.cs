using Abp.Application.Services.Dto;

namespace DanielASG.HelpDesk.Tickets.Dtos
{
    public class StandardMessageDto : EntityDto
    {
        public string Description { get; set; }

        public string MessageType { get; set; }

        public bool IsActive { get; set; }

    }
}