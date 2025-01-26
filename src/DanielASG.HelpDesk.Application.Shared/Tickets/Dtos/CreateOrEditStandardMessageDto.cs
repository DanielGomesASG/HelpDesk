using Abp.Application.Services.Dto;

namespace DanielASG.HelpDesk.Tickets.Dtos
{
    public class CreateOrEditStandardMessageDto : EntityDto<int?>
    {
        public int? MessageTypeId { get; set; }

        public string Description { get; set; }

        public string Message { get; set; }

        public bool IsActive { get; set; }

    }
}