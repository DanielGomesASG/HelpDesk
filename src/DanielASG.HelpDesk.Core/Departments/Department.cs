using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanielASG.HelpDesk.Departments
{
    [Table("Departments")]
    public class Department : FullAuditedEntity, IMustHaveTenant, IMustHaveIsActive
    {
        public int TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual bool IsActive { get; set; }
    }
}