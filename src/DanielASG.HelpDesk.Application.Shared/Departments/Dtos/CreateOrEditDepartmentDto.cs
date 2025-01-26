using Abp.Application.Services.Dto;

namespace DanielASG.HelpDesk.Departments.Dtos
{
    public class CreateOrEditDepartmentDto : EntityDto<int?>
    {

        public string Name { get; set; }

        public string Code { get; set; }

        public bool IsActive { get; set; }

    }
}