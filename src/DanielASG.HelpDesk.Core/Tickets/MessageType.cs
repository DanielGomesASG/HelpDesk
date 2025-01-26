using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanielASG.HelpDesk.Tickets
{
    [Table("MessageTypes")]
    public class MessageType : EntityBase, IMustBaseEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string Instructions { get; set; }

        public virtual bool IsActive { get; set; }

    }
}