using System.Collections.Generic;
using Abp;
using DanielASG.HelpDesk.Chat.Dto;
using DanielASG.HelpDesk.Dto;

namespace DanielASG.HelpDesk.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
