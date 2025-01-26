using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Base;

namespace DanielASG.HelpDesk
{
    public class BaseCrudAppService<TEntity, TEntityDto> : AsyncCrudAppService<TEntity, TEntityDto>
     where TEntity : class, IEntity<int>, IMustBaseEntity
     where TEntityDto : IEntityDto<int>, IMustBaseEntity
    {

        public readonly IRepository<TEntity, int> _repository;
        protected BaseCrudAppService(IRepository<TEntity, int> repository)
            : base(repository)
        {
            _repository = repository;
            LocalizationSourceName = HelpDeskConsts.LocalizationSourceName;
        }

        public async virtual Task CreateOrEditRange(List<TEntityDto> items)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete, CustomFilterConsts.IsActive))
            {
                foreach (var item in items)
                {
                    var entity = await _repository.GetAll()
                                                  .Where(a => a.Code == item.Code)
                                                  .FirstOrDefaultAsync();
                    if (entity != null)
                        item.Id = entity.Id;

                    if (item.Id == 0)
                    {
                        await base.CreateAsync(item);
                    }
                    else
                    {
                        await base.UpdateAsync(item);
                    }

                    await CurrentUnitOfWork.SaveChangesAsync();
                }
            }
        }

        public async virtual Task<PagedResultDto<TEntityDto>> GetAllFilterAsync(GetAllInput input)
        {
            using (CurrentUnitOfWork.DisableFilter(CustomFilterConsts.IsActive))
            {
                var query = base.CreateFilteredQuery(input)
                                .WhereIf(!input.FilterText.IsNullOrEmpty(),
                                        a => a.Code.Contains(input.FilterText) ||
                                             a.Name.Contains(input.FilterText)
                                );

                var totalCount = await query.CountAsync();

                query = ApplySorting(query, input);
                query = ApplyPaging(query, input);

                var entities = await query.ToListAsync();

                return new PagedResultDto<TEntityDto>(
                    totalCount,
                    entities.Select(MapToEntityDto).ToList()
                );
            }
        }

        public async virtual Task<BaseSelectInputDto[]> GetAllForSelectInput(string filterText, int?[] initialId)
        {
            var query = _repository.GetAll()
                                    .AsNoTracking()
                                    .WhereIf(initialId != null && initialId.Any(), x => initialId.Contains(x.Id))
                                    .WhereIf(!filterText.IsNullOrEmpty(),
                                            a => a.Code.Contains(filterText) ||
                                                a.Name.Contains(filterText));

            var isActive = typeof(TEntity).GetProperty("IsActive");
            if (isActive != null)
                query = query.Where("IsActive = @0", true);

            var items = await query.Take(50)
                                   .Select(a => new BaseSelectInputDto()
                                   {
                                       Id = a.Id,
                                       Name = a.Name,
                                       Code = a.Code
                                   })
                                   .ToArrayAsync();

            return items;
        }

        public async override Task<TEntityDto> GetAsync(EntityDto<int> input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete, CustomFilterConsts.IsActive))
            {
                return await base.GetAsync(input);
            }
        }

        public async virtual Task<TEntityDto> CreateOrEdit(TEntityDto model)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete, CustomFilterConsts.IsActive))
            {
                if (model.Id == 0)
                    return await base.CreateAsync(model);
                else
                    return await base.UpdateAsync(model);
            }
        }

        protected static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
        }

        protected static string RemoveDiacritics(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString().ToUpper();
        }
    }
}
