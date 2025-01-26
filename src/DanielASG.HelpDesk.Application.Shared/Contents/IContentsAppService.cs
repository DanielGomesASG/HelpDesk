using Abp.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Contents.Dtos;

namespace DanielASG.HelpDesk.Contents
{
    public interface IContentsAppService : IApplicationService
    {
        Task<List<ContentForHomeDto>> GetAllForHome();
    }
}