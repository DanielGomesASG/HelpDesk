﻿using System.Threading.Tasks;
using Abp.Application.Services;
using DanielASG.HelpDesk.Install.Dto;

namespace DanielASG.HelpDesk.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}