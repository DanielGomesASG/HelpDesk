using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Authorization;
using DanielASG.HelpDesk.Base;
using DanielASG.HelpDesk.Tickets.Dtos;

namespace DanielASG.HelpDesk.Tickets
{
    [AbpAuthorize(AppPermissions.Pages_Register_Tickets_Statuses, AppPermissions.Pages_Tickets)]
    public class StatusesAppService : BaseCrudAppService<Status, StatusDto>, IStatusesAppService
    {
        private readonly IRepository<Status> _statusRepository;

        public StatusesAppService(IRepository<Status> statusRepository)
        : base(statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public override async Task<BaseSelectInputDto[]> GetAllForSelectInput(string filterText, int?[] initialId)
        {
            var query = _repository.GetAll()
                                    .AsNoTracking()
                                    .WhereIf(initialId != null && initialId.Any(), x => initialId.Contains(x.Id))
                                    .WhereIf(!filterText.IsNullOrEmpty(),
                                            a => a.Code.Contains(filterText) ||
                                                a.Name.Contains(filterText));

            var isActive = typeof(IEntity).GetProperty("IsActive");
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

    }
}