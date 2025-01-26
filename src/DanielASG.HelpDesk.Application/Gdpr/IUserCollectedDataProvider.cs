using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using DanielASG.HelpDesk.Dto;

namespace DanielASG.HelpDesk.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
