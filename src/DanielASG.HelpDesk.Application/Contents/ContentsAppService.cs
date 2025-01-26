using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Timing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Base;
using DanielASG.HelpDesk.Contents.Dtos;

namespace DanielASG.HelpDesk.Contents
{
    public class ContentsAppService : BaseCrudAppService<Content, ContentDto>, IContentsAppService
    {
        private readonly IRepository<Content> _contentRepository;
        private readonly IDapperRepository<Content> _contentDapperRepository;

        public ContentsAppService(IRepository<Content> contentRepository,
                                  IDapperRepository<Content> contentDapperRepository) :
            base(contentRepository)
        {
            _contentRepository = contentRepository;
            _contentDapperRepository = contentDapperRepository;
        }

        public async override Task<ContentDto> GetAsync(EntityDto<int> input)
        {
            var content = await base.GetAsync(input);

            if (!content.Roles.IsNullOrEmpty())
                content.RoleIds = content.Roles.Split('|')?.Select(x => x.To<int>()).ToArray();

            return content;
        }

        public async override Task<ContentDto> CreateOrEdit(ContentDto model)
        {
            if (model.RoleIds != null && model.RoleIds.Any())
                model.Roles = string.Join('|', model.RoleIds);
            else
                model.Roles = string.Empty;

            return await base.CreateOrEdit(model);
        }

        public async override Task<PagedResultDto<ContentDto>> GetAllFilterAsync(GetAllInput input)
        {
            using (CurrentUnitOfWork.DisableFilter(CustomFilterConsts.IsActive))
            {
                var query = base.CreateFilteredQuery(input)
                                .WhereIf(!input.FilterText.IsNullOrEmpty(),
                                             a => a.Name.Contains(input.FilterText) ||
                                             a.Code.Contains(input.FilterText));

                var totalCount = await query.CountAsync();

                query = ApplySorting(query, input);
                query = ApplyPaging(query, input);

                var entities = await query.Select(a => new ContentDto()
                {
                    Id = a.Id,
                    Name = a.Name,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    IsActive = a.IsActive,
                }).ToListAsync();

                return new PagedResultDto<ContentDto>(
                    totalCount,
                    entities
                );
            }
        }

        public async Task<List<ContentForHomeDto>> GetAllForHome()
        {
            var now = Clock.Now;
            var query = await _contentDapperRepository.QueryAsync<ContentForHomeDto>(
            $@"
                SELECT
                C.Id,
                C.Name,
                C.ContentHtml as Html
                FROM
                Contents C
                WHERE
                (C.EndDate IS NULL OR C.EndDate >= @now) AND
                (C.StartDate IS NULL OR C.StartDate <= @now) AND
                ((C.Roles IS NULL OR LEN(C.Roles) = 0) OR
                EXISTS 
                (
	                SELECT 1
                    FROM (select RoleId from AbpUserRoles where UserId = @userId) UR
                    WHERE CHARINDEX('|' + CAST(UR.RoleId AS NVARCHAR) + '|', '|' + C.Roles + '|') > 0
                ))",
                new
                {
                    now = now.Date,
                    userId = AbpSession.UserId.Value
                });

            return query.ToList();
        }
    }
}