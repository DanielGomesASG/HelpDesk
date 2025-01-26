using Abp.Application.Services.Dto;

namespace DanielASG.HelpDesk.Tickets.Dtos
{
    public class StatusDto : EntityDto, IMustBaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        public string BackgroundColor { get; set; }

        public bool IsActive { get; set; }

    }
}