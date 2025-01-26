using System.Collections.Generic;
using DanielASG.HelpDesk.Auditing.Dto;
using DanielASG.HelpDesk.Dto;

namespace DanielASG.HelpDesk.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
