using Abp.Application.Services.Dto;
using System;

namespace DanielASG.HelpDesk.Contents.Dtos
{
    public class CreateOrEditContentDto : EntityDto<int?>
    {

        public string Name { get; set; }

        public string ContentHtml { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Ordem { get; set; }

        public bool IsActive { get; set; }

    }
}