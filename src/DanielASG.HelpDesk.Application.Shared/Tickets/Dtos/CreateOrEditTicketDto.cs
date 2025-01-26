using Abp.Application.Services.Dto;
using System;
using DanielASG.HelpDesk.Authorization.Users.Dto;
using DanielASG.HelpDesk.Departments.Dtos;

namespace DanielASG.HelpDesk.Tickets.Dtos
{
    public class CreateOrEditTicketDto : EntityDto<int?>
    {
        public long? CustomerUserId { get; set; }
        public UserListDto CustomerUser { get; set; }

        public long? StaffUserId { get; set; }
        public UserListDto StaffUser { get; set; }

        public int? MessageTypeId { get; set; }
        public MessageTypeDto MessageType { get; set; }

        public int? DepartmentId { get; set; }
        public DepartmentDto Department { get; set; }

        public int? StatusId { get; set; }
        public StatusDto Status { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public bool Notify { get; set; }

        public Guid? Files { get; set; }

        public string FilesToken { get; set; }

        public DateTime? CreationTime { get; set; }

        public DateTime? OpenDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public string Priority { get; set; }

        public EOrigin Origin { get; set; }

        public string MessageUniqueId { get; set; }

    }
}