using System.Collections.Generic;
using Abp.Dependency;
using DanielASG.HelpDesk.Dto;

namespace DanielASG.HelpDesk.DataImporting.Excel;

public interface IExcelInvalidEntityExporter<TEntityDto> : ITransientDependency
{
    FileDto ExportToFile(List<TEntityDto> entities);
}