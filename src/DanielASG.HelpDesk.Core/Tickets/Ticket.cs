using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using DanielASG.HelpDesk.Authorization.Users;
using DanielASG.HelpDesk.Departments;

namespace DanielASG.HelpDesk.Tickets
{
    [Table("Tickets")]
    public class Ticket : FullAuditedEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual int? MessageTypeId { get; set; }
        [ForeignKey("MessageTypeId")]
        public virtual MessageType MessageType { get; set; }

        public virtual string Subject { get; set; }

        public virtual string Message { get; set; }

        public virtual bool Notify { get; set; }
        //File

        public virtual Guid? Files { get; set; } //File, (BinaryObjectId)

        public virtual int? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }

        public virtual long? CustomerUserId { get; set; }
        [ForeignKey("CustomerUserId")]
        public virtual User CustomerUser { get; set; }

        public virtual long? StaffUserId { get; set; }
        [ForeignKey("StaffUserId")]
        public virtual User StaffUser { get; set; }

        public virtual int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual DateTime? OpenDate { get; set; }
        public virtual DateTime? FinishDate { get; set; }

        public virtual string Priority { get; set; }

        public virtual EOrigin Origin { get; set; }
    }
}