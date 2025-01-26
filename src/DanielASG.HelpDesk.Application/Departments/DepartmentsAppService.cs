using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dapper.Repositories;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Authorization;
using DanielASG.HelpDesk.Authorization.Users;
using DanielASG.HelpDesk.Base;
using DanielASG.HelpDesk.Departments.Dtos;

namespace DanielASG.HelpDesk.Departments
{
    [AbpAuthorize(AppPermissions.Pages_Departments, AppPermissions.Pages_Tickets)]
    public class DepartmentsAppService : HelpDeskAppServiceBase, IDepartmentsAppService
    {
        private readonly IRepository<UserDepartment> _userDepartmentRepository;
        private readonly IRepository<Department> _departmentRepository;

        private readonly IDapperRepository<Department, int> _dapperDepartmentRepository;

        public DepartmentsAppService(
                                    IRepository<UserDepartment> userDepartmentRepository,
                                    IRepository<Department> departmentRepository,

                                    IDapperRepository<Department, int> dapperDepartmentRepository)
        {
            _userDepartmentRepository = userDepartmentRepository;
            _departmentRepository = departmentRepository;

            _dapperDepartmentRepository = dapperDepartmentRepository;
        }

        public async Task<PagedResultDto<DepartmentDto>> GetAll(GetAllDepartmentsInput input)
        {
            var queryText =
                $@"
                    SELECT
                        D.*
                    FROM
                        Departments D
                    WHERE (
                        D.IsDeleted = 0 AND
                        D.TenantId = @tenantId AND
                        (
                            @filterText IS NULL OR 
                            (
                                (CHARINDEX(@filterText, D.Name) > 0) OR
                                (@filterText = N'')
                            )
                        ) 
                    )
                    ORDER BY {input.Sorting}
                    OFFSET @skipCount ROWS FETCH NEXT @maxResultCount ROWS ONLY
                ";

            var param = new
            {
                filterText = input.Filter,
                tenantId = AbpSession.TenantId,

                skipCount = input.SkipCount,
                maxResultCount = input.MaxResultCount
            };

            var query = await _dapperDepartmentRepository.QueryAsync<DepartmentDto>(queryText, param);

            queryText = queryText.Substring(queryText.IndexOf("FROM"));
            queryText = queryText.Substring(0, queryText.LastIndexOf("ORDER BY"));
            queryText = $"SELECT COUNT(*) as TotalCount  {queryText}";

            var pagedResult = _dapperDepartmentRepository.Query<PagedResultDto<DepartmentDto>>(queryText, param).FirstOrDefault();
            pagedResult.Items = query.ToList();

            return pagedResult;
        }

        [AbpAuthorize(AppPermissions.Pages_Departments_Edit)]
        public async Task<CreateOrEditDepartmentDto> GetDepartmentForEdit(EntityDto input)
        {
            var department = await _departmentRepository.FirstOrDefaultAsync(input.Id);

            var output = ObjectMapper.Map<CreateOrEditDepartmentDto>(department);

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDepartmentDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        public async Task<BaseSelectInputDto[]> GetAllForSelectInput(string filterText, int?[] initialId)
        {
            var userDepartmentIds = await _userDepartmentRepository.GetAll()
                                                                   .Where(x => x.UserId == AbpSession.UserId)
                                                                   .Select(x => x.DepartmentId)
                                                                   .ToListAsync();

            var query = _departmentRepository.GetAll()
                                             .AsNoTracking()
                                             .WhereIf(initialId != null && initialId.Any(), x => initialId.Contains(x.Id))
                                             .WhereIf(!filterText.IsNullOrEmpty(),
                                                a => a.Code.Contains(filterText) ||
                                                a.Name.Contains(filterText))
                                             .WhereIf(!userDepartmentIds.IsNullOrEmpty(), x => userDepartmentIds.Contains(x.Id));

            var isActive = typeof(IEntity).GetProperty("IsActive");
            if (isActive != null)
                query = query.WhereDynamic("IsActive = @0", true);

            var items = await query.Take(50)
                                   .Select(a => new BaseSelectInputDto()
                                   {
                                       Id = a.Id,
                                       Name = a.Name,
                                       Code = a.Code
                                   })
                                   .ToDynamicArrayAsync();

            return [.. items];
        }

        [AbpAuthorize(AppPermissions.Pages_Departments_Create)]
        protected virtual async Task Create(CreateOrEditDepartmentDto input)
        {
            var department = ObjectMapper.Map<Department>(input);

            if (AbpSession.TenantId != null)
            {
                department.TenantId = (int)AbpSession.TenantId;
            }

            await _departmentRepository.InsertAsync(department);

        }

        [AbpAuthorize(AppPermissions.Pages_Departments_Edit)]
        protected virtual async Task Update(CreateOrEditDepartmentDto input)
        {
            var department = await _departmentRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, department);

        }

        [AbpAuthorize(AppPermissions.Pages_Departments_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _departmentRepository.DeleteAsync(input.Id);
        }

    }
}