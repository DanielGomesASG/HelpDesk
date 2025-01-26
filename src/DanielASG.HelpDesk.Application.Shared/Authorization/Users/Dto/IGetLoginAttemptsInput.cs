using Abp.Application.Services.Dto;

namespace DanielASG.HelpDesk.Authorization.Users.Dto
{
    public interface IGetLoginAttemptsInput: ISortedResultRequest
    {
        string Filter { get; set; }
    }
}