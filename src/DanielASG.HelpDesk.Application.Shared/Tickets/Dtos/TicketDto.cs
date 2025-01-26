using Abp.Application.Services.Dto;
using DanielASG.HelpDesk.Authorization.Users.Dto;
using DanielASG.HelpDesk.Departments.Dtos;

namespace DanielASG.HelpDesk.Tickets.Dtos
{
    public class TicketDto : EntityDto
    {
        public string Priority { get; set; }

        public string Subject { get; set; }

        public string CustomerUserName { get; set; }
        public UserEditDto CustomerUser { get; set; }

        public string StaffUserName { get; set; }
        public UserEditDto StaffUser { get; set; }

        public string DepartmentName { get; set; }
        public DepartmentDto Department { get; set; }

        public string MessageTypeName { get; set; }
        public MessageTypeDto MessageType { get; set; }

        public string StatusName { get; set; }
        public string StatusCode { get; set; }
        public string StatusColor { get; set; }
        public string StatusBackgroundColor { get; set; }
        public StatusDto Status { get; set; }

    }
}