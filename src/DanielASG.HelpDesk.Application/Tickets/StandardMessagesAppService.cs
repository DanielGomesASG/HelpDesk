using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Authorization;
using DanielASG.HelpDesk.Base;
using DanielASG.HelpDesk.Tickets.Dtos;

namespace DanielASG.HelpDesk.Tickets
{
    [AbpAuthorize(AppPermissions.Pages_Register_Tickets_StandardMessages)]
    public class StandardMessagesAppService : HelpDeskAppServiceBase, IStandardMessagesAppService
    {
        private readonly IRepository<StandardMessage> _standardMessageRepository;

        public StandardMessagesAppService(IRepository<StandardMessage> standardMessageRepository)
        {
            _standardMessageRepository = standardMessageRepository;

        }

        public async Task<PagedResultDto<StandardMessageDto>> GetAll(GetAllStandardMessagesInput input)
        {

            var filteredStandardMessages = _standardMessageRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter) || e.Message.Contains(input.Filter));

            var pagedAndFilteredStandardMessages = filteredStandardMessages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var standardMessages = from o in pagedAndFilteredStandardMessages
                                   select new
                                   {
                                       o.Description,
                                       o.MessageType,
                                       o.IsActive,
                                       Id = o.Id
                                   };

            var totalCount = await filteredStandardMessages.CountAsync();

            var dbList = await standardMessages.ToListAsync();
            var results = new List<StandardMessageDto>();

            foreach (var o in dbList)
            {
                var res = new StandardMessageDto()
                {
                    Description = o.Description,
                    MessageType = o.MessageType?.Name,
                    IsActive = o.IsActive,
                    Id = o.Id,
                };

                results.Add(res);
            }

            return new PagedResultDto<StandardMessageDto>(
                totalCount,
                results
            );

        }

        public async virtual Task<BaseSelectInputDto[]> GetAllForSelectInput(string filterText, long?[] initialId, int? messageTypeId)
        {
            var query = _standardMessageRepository.GetAll()
                                                  .AsNoTracking()
                                                  .WhereIf(initialId != null && initialId.Any(), x => initialId.Contains(x.Id))
                                                  .Where(x => x.MessageTypeId == messageTypeId || x.MessageTypeId == null);


            var items = await query.Take(50)
                                   .Select(a => new BaseSelectInputDto()
                                   {
                                       Id = a.Id,
                                       Name = a.Description,
                                       Code = a.Message
                                   })
                                   .ToArrayAsync();

            return items;
        }

        [AbpAuthorize(AppPermissions.Pages_Register_Tickets_StandardMessages_Edit)]
        public async Task<CreateOrEditStandardMessageDto> GetStandardMessageForEdit(EntityDto input)
        {
            var standardMessage = await _standardMessageRepository.FirstOrDefaultAsync(input.Id);

            var output = ObjectMapper.Map<CreateOrEditStandardMessageDto>(standardMessage);

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStandardMessageDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Register_Tickets_StandardMessages_Create)]
        protected virtual async Task Create(CreateOrEditStandardMessageDto input)
        {
            var standardMessage = ObjectMapper.Map<StandardMessage>(input);

            if (AbpSession.TenantId != null)
            {
                standardMessage.TenantId = (int)AbpSession.TenantId;
            }

            await _standardMessageRepository.InsertAsync(standardMessage);

        }

        [AbpAuthorize(AppPermissions.Pages_Register_Tickets_StandardMessages_Edit)]
        protected virtual async Task Update(CreateOrEditStandardMessageDto input)
        {
            var standardMessage = await _standardMessageRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, standardMessage);

        }

        [AbpAuthorize(AppPermissions.Pages_Register_Tickets_StandardMessages_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _standardMessageRepository.DeleteAsync(input.Id);
        }

    }
}