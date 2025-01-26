using System.Collections.Generic;
using Abp.Dependency;

namespace DanielASG.HelpDesk.DataImporting.Excel;

public interface IExcelDataReader<TEntityDto> : ITransientDependency
{
    List<TEntityDto> GetEntitiesFromExcel(byte[] fileBytes);
}