using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanielASG.HelpDesk.Tickets
{
    [Table("Statuses")]
    public class Status : EntityBase, IMustBaseEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string Color { get; set; }

        public virtual string BackgroundColor { get; set; }

        public virtual bool IsActive { get; set; }

    }
}