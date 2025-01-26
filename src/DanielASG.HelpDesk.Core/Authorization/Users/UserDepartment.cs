using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using DanielASG.HelpDesk.Departments;

namespace DanielASG.HelpDesk.Authorization.Users
{
    [Table("UserDepartments")]
    public class UserDepartment : FullAuditedEntity
    {
        public virtual long? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public virtual int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
    }
}
