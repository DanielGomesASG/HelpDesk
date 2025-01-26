﻿using Abp.Application.Services;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Base;

namespace DanielASG.HelpDesk.Tickets
{
    public interface IMessageTypesAppService : IApplicationService
    {
        Task<BaseSelectInputDto[]> GetAllForSelectInput(string filterText, int?[] initialId);

    }
}