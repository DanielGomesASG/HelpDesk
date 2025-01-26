using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanielASG.HelpDesk.Tickets
{
    [Table("TicketMessages")]
    public class TicketMessage : AuditedEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual string Message { get; set; }

        public virtual int? TicketId { get; set; }
        [ForeignKey("TicketId")]
        public virtual Ticket Ticket { get; set; }

        public virtual EOrigin Origin { get; set; }

        public virtual string UniqueId { get; set; }
    }
}
