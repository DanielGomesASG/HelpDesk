using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace DanielASG.HelpDesk.Departments.Dtos
{
    public class GetAllDepartmentsInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "D.Id DESC";
            }
        }
    }
}