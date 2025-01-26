using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Authorization;
using DanielASG.HelpDesk.Base;
using DanielASG.HelpDesk.Tickets.Dtos;

namespace DanielASG.HelpDesk.Tickets
{
    [AbpAuthorize(AppPermissions.Pages_Tickets)]
    public class MessageTypesAppService : BaseCrudAppService<MessageType, MessageTypeDto>, IMessageTypesAppService
    {
        private readonly IRepository<MessageType> _messageTypeRepository;

        public MessageTypesAppService(IRepository<MessageType> messageTypeRepository)
        : base(messageTypeRepository)
        {
            _messageTypeRepository = messageTypeRepository;
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
                                       Code = a.Instructions,
                                   })
                                   .ToArrayAsync();

            return items;
        }

    }
}