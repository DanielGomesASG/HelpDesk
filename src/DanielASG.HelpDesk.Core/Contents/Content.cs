using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanielASG.HelpDesk.Contents
{
    [Table("Contents")]
    public class Content : EntityBase, IMustBaseEntity, IMustHaveTenant, IMustHaveIsActive
    {
        public int TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string ContentHtml { get; set; }

        public virtual DateTime? StartDate { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual int Ordem { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual string Roles { get; set; }

    }
}