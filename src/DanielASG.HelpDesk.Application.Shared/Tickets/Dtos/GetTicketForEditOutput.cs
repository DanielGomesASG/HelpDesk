using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DanielASG.HelpDesk.Tickets.Dtos
{
    public class GetTicketForEditOutput
    {
        public CreateOrEditTicketDto Ticket { get; set; }

        public string FilesFileName { get; set; }

    }
}