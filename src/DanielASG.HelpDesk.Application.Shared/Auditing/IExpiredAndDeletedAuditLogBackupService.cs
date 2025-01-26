using System.Collections.Generic;
using Abp.Auditing;

namespace DanielASG.HelpDesk.Auditing
{
    public interface IExpiredAndDeletedAuditLogBackupService
    {
        bool CanBackup();
        
        void Backup(List<AuditLog> auditLogs);
    }
}