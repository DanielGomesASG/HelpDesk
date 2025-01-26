using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanielASG.HelpDesk.Tickets
{
    [Table("StandardMessages")]
    public class StandardMessage : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual string Description { get; set; }

        public virtual string Message { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int? MessageTypeId { get; set; }
        [ForeignKey("MessageTypeId")]
        public virtual MessageType MessageType { get; set; }

    }
}