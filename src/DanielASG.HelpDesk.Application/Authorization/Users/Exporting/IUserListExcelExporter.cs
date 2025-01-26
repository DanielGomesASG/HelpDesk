using System.Collections.Generic;
using DanielASG.HelpDesk.Authorization.Users.Dto;
using DanielASG.HelpDesk.Dto;

namespace DanielASG.HelpDesk.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos, List<string> selectedColumns);
    }
}