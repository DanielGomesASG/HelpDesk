using Abp.Application.Services.Dto;

namespace DanielASG.HelpDesk.Departments.Dtos
{
    public class DepartmentDto : EntityDto
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public bool IsActive { get; set; }

    }
}