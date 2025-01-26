using Abp.Application.Services.Dto;
using System;

namespace DanielASG.HelpDesk.Contents.Dtos
{
    public class ContentDto : EntityDto, IMustBaseEntity
    {
        public string Name { get; set; }
        public string ContentHtml { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Ordem { get; set; }

        public bool IsActive { get; set; }

        public string Code { get; set; }

        public string Roles { get; set; }
        public int[] RoleIds { get; set; }
    }
}