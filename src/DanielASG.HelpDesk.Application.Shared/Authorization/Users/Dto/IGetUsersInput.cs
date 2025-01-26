using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace DanielASG.HelpDesk.Authorization.Users.Dto
{
    public interface IGetUsersInput : ISortedResultRequest
    {
        string Filter { get; set; }

        EProfileType ProfileType { get; set; }

        List<string> Permissions { get; set; }

        int? Role { get; set; }

        bool OnlyLockedUsers { get; set; }
    }
}
