using Abp.Application.Services.Dto;

namespace DanielASG.HelpDesk.Tickets.Dtos
{
    public class MessageTypeDto : EntityDto, IMustBaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Instructions { get; set; }

        public string Code { get; set; }

        public bool IsActive { get; set; }

    }
}