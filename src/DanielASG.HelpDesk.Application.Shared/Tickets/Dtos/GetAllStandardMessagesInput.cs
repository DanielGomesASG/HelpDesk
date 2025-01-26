using Abp.Application.Services.Dto;
using System;

namespace DanielASG.HelpDesk.Tickets.Dtos
{
    public class GetAllStandardMessagesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

    }
}