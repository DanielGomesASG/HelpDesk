using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dapper.Repositories;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Authorization;
using DanielASG.HelpDesk.Authorization.Users;
using DanielASG.HelpDesk.Storage;
using DanielASG.HelpDesk.Tickets.Dtos;

namespace DanielASG.HelpDesk.Tickets
{
    [AbpAuthorize(AppPermissions.Pages_Tickets)]
    public class TicketsAppService : HelpDeskAppServiceBase, ITicketsAppService
    {
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<Status> _statusRepository;
        private readonly IRepository<TicketMessage> _ticketMessageRepository;
        private readonly IRepository<UserDepartment> _userDepartmentRepository;

        private readonly IDapperRepository<Ticket> _ticketDapperRepository;

        private readonly ITicketEmailer _ticketEmailer;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public TicketsAppService(
            IRepository<Ticket> ticketRepository,
            IRepository<Status> statusRepository,
            IRepository<TicketMessage> ticketMessageRepository,
            IRepository<UserDepartment> userDepartmentRepository,

            IDapperRepository<Ticket> ticketDapperRepository,

            ITicketEmailer ticketEmailer,
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager)
        {
            _ticketRepository = ticketRepository;
            _statusRepository = statusRepository;
            _ticketMessageRepository = ticketMessageRepository;
            _userDepartmentRepository = userDepartmentRepository;

            _ticketDapperRepository = ticketDapperRepository;

            _ticketEmailer = ticketEmailer;
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;

        }

        public async Task<PagedResultDto<TicketDto>> GetAll(GetAllTicketsInput input)
        {
            var userDepartmentIds = await _userDepartmentRepository.GetAll()
                                                                   .Where(x => x.UserId == AbpSession.UserId)
                                                                   .Select(x => x.DepartmentId)
                                                                   .ToListAsync();

            var user = UserManager.GetUserById((long)AbpSession.UserId);

            var queryText =
             $@" 
                SELECT
                    T.*,
                    S.Name as StatusName,
                    S.Code as StatusCode,
                    S.Color as StatusColor,
                    S.BackgroundColor as StatusBackgroundColor,
                    MT.Name as MessageTypeName,
                    UC.Name + ' ' + UC.Surname as CustomerUserName,
                    US.Name + ' ' + US.Surname as StaffUserName,
                    D.Name as DepartmentName
                FROM
                    Tickets T
                    LEFT JOIN Statuses S ON T.StatusId = S.Id
                    LEFT JOIN MessageTypes MT ON T.MessageTypeId = MT.Id
                    LEFT JOIN AbpUsers UC ON T.CustomerUserId = UC.Id
                    LEFT JOIN AbpUsers US ON T.StaffUserId = US.Id
                    LEFT JOIN Departments D ON T.DepartmentId = D.Id
                WHERE (
                    T.TenantId = @tenantId AND
                    T.IsDeleted = 0 AND 
                    {(input.UserId.HasValue ? "(T.StaffUserId = @userId OR T.CustomerUserId = @userId) AND" : "")}
                    {(!userDepartmentIds.IsNullOrEmpty() ? (!input.UserId.HasValue && user.ProfileType == EProfileType.Staff ?
                    "D.Id IN @userDepartments OR T.DepartmentId IS NULL AND" : "D.Id IN @userDepartments AND") : "")}
                    (
                        @filterText IS NULL OR 
                        (
                            (CHARINDEX(@filterText, T.[Id]) > 0) OR 
                            (CHARINDEX(@filterText, T.[Subject]) > 0) OR
                            (CHARINDEX(@filterText, MT.[Name]) > 0) OR
                            (CHARINDEX(@filterText, S.[Name]) > 0) OR
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
                userId = input.UserId,
                userDepartments = userDepartmentIds,

                skipCount = input.SkipCount,
                maxResultCount = input.MaxResultCount
            };

            var query = await _ticketDapperRepository.QueryAsync<TicketDto>(queryText, param);

            queryText = queryText.Substring(queryText.IndexOf("FROM"));
            queryText = queryText.Substring(0, queryText.LastIndexOf("ORDER BY"));
            queryText = $"SELECT COUNT(*) as TotalCount  {queryText}";

            var pagedResult = _ticketDapperRepository.Query<PagedResultDto<TicketDto>>(queryText, param).FirstOrDefault();
            pagedResult.Items = query.ToList();

            return pagedResult;
        }

        public async Task<GetTicketDetailsOutput> GetTicketDetails(EntityDto input)
        {
            var ticket = await _ticketRepository.GetAll().AsNoTracking()
                                                .Where(x => x.Id == input.Id)
                                                .Include("CustomerUser")
                                                .Include("StaffUser")
                                                .Include("Department")
                                                .Include("Status")
                                                .Include("MessageType")
                                                .FirstOrDefaultAsync();

            var messages = await _ticketMessageRepository.GetAll()
                                                         .Where(x => x.TicketId == input.Id)
                                                         .OrderBy(x => x.CreationTime)
                                                         .ToListAsync();

            var output = new GetTicketDetailsOutput
            {
                Ticket = ObjectMapper.Map<CreateOrEditTicketDto>(ticket),
                Messages = ObjectMapper.Map<List<TicketMessageDto>>(messages),
                FilesFileName = await GetBinaryFileName(ticket.Files)
            };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Tickets_Edit)]
        public async Task<GetTicketForEditOutput> GetTicketForEdit(EntityDto input)
        {
            var ticket = await _ticketRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTicketForEditOutput { Ticket = ObjectMapper.Map<CreateOrEditTicketDto>(ticket) };

            output.FilesFileName = await GetBinaryFileName(ticket.Files);

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTicketDto input)
        {
            if (input.Id == null)
            {
                input.Id = await Create(input);

                await InsertTicketMessage(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Tickets_Create)]
        protected virtual async Task<int> Create(CreateOrEditTicketDto input)
        {
            var ticket = ObjectMapper.Map<Ticket>(input);

            if (!ticket.StatusId.HasValue)
            {
                ticket.StatusId = _statusRepository.FirstOrDefault(x => x.Code == EStatus.AguardandoAtendente)?.Id;
            }

            if (AbpSession.TenantId != null)
            {
                ticket.TenantId = (int)AbpSession.TenantId;
            }

            var ticketId = await _ticketRepository.InsertAndGetIdAsync(ticket);
            ticket.Files = await GetBinaryObjectFromCache(input.FilesToken);

            return ticketId;
        }

        [AbpAuthorize(AppPermissions.Pages_Tickets_Edit)]
        protected virtual async Task Update(CreateOrEditTicketDto input)
        {
            var ticket = await _ticketRepository.FirstOrDefaultAsync((int)input.Id);

            if (input.Message != ticket.Message)
            {
                var message = await _ticketMessageRepository.GetAll()
                                                            .Where(x => x.Id == ticket.Id)
                                                            .OrderBy(x => x.CreationTime)
                                                            .FirstOrDefaultAsync();

                message.Message = ticket.Message;
            }

            if (ticket.Notify && input.StatusId != ticket.StatusId)
                await _ticketEmailer.SendStatusEmail((int)AbpSession.TenantId, ticket.Id, (int)ticket.StatusId, (int)input.StatusId);

            ObjectMapper.Map(input, ticket);
            ticket.Files = await GetBinaryObjectFromCache(input.FilesToken);
        }

        [AbpAuthorize(AppPermissions.Pages_Tickets_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _ticketRepository.DeleteAsync(input.Id);

            var messages = await _ticketMessageRepository.GetAll()
                                                         .Where(x => x.TicketId == input.Id)
                                                         .ToListAsync();

            foreach (var message in messages)
            {
                await _ticketMessageRepository.DeleteAsync(message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Tickets_Bind)]
        public async Task Bind(CreateOrEditTicketDto input)
        {
            var ticket = await _ticketRepository.FirstOrDefaultAsync((int)input.Id);

            var oldStatusId = ticket.StatusId;

            ObjectMapper.Map(input, ticket);

            ticket.Files = await GetBinaryObjectFromCache(input.FilesToken);

            await CurrentUnitOfWork.SaveChangesAsync();

            var newStatusId = _statusRepository.FirstOrDefault(x => x.Code == EStatus.AtendenteVinculado).Id;

            if (ticket.Notify)
                await _ticketEmailer.SendStatusEmail((int)AbpSession.TenantId, ticket.Id, (int)oldStatusId, newStatusId);

            ticket.StatusId = newStatusId;
        }

        public async Task Finish(CreateOrEditTicketDto input)
        {
            input.CustomerUser = null;
            input.StaffUser = null;
            input.MessageType = null;
            input.Department = null;
            input.Status = null;

            var ticket = await _ticketRepository.FirstOrDefaultAsync((int)input.Id);

            var oldStatusId = ticket.StatusId;
            var newStatusId = _statusRepository.FirstOrDefault(x => x.Code == EStatus.Finalizado).Id;

            if (ticket.Notify)
                await _ticketEmailer.SendStatusEmail((int)AbpSession.TenantId, ticket.Id, (int)oldStatusId, newStatusId);

            input.StatusId = newStatusId;
            input.FinishDate = DateTime.Now;

            ObjectMapper.Map(input, ticket);
            ticket.Files = await GetBinaryObjectFromCache(input.FilesToken);
        }

        public async Task InsertTicketMessage(CreateOrEditTicketDto input)
        {
            var ticketMessages = await _ticketMessageRepository.GetAll().Where(x => x.TicketId == input.Id).ToListAsync();
            var ticket = await _ticketRepository.FirstOrDefaultAsync((int)input.Id);

            if (!ticketMessages.IsNullOrEmpty() && !ticketMessages.Select(x => x.CreatorUserId).Contains(ticket.StaffUserId) && AbpSession.UserId == ticket.StaffUserId)
            {
                if (!ticket.IsNullOrDeleted())
                {
                    var oldStatusId = ticket.StatusId;
                    var newStatusId = _statusRepository.FirstOrDefault(x => x.Code == EStatus.Aberto).Id;

                    if (ticket.Notify)
                        await _ticketEmailer.SendStatusEmail((int)AbpSession.TenantId, ticket.Id, (int)oldStatusId, newStatusId);

                    ticket.StatusId = newStatusId;
                    ticket.OpenDate = DateTime.Now;
                }
            }

            await _ticketMessageRepository.InsertAsync(new TicketMessage()
            {
                TenantId = (int)AbpSession.TenantId,
                Message = input.Message,
                TicketId = input.Id,
            });

            await CurrentUnitOfWork.SaveChangesAsync();

            if (ticket.Notify)
            {
                if (ticket.Origin == EOrigin.Internal)
                    await _ticketEmailer.SendDefaultMessageEmail((int)AbpSession.TenantId, ticket.Id, input.Message);

                if (ticket.Origin == EOrigin.Email)
                    await _ticketEmailer.SendMessageEmail((int)AbpSession.TenantId, ticket.Id, input.Message);
            }
        }

        private async Task<Guid?> GetBinaryObjectFromCache(string fileToken)
        {
            if (fileToken.IsNullOrWhiteSpace())
            {
                return null;
            }

            var fileCache = _tempFileCacheManager.GetFileInfo(fileToken);

            if (fileCache == null)
            {
                throw new UserFriendlyException("There is no such file with the token: " + fileToken);
            }

            var storedFile = new BinaryObject(AbpSession.TenantId, fileCache.File, fileCache.FileName);
            await _binaryObjectManager.SaveAsync(storedFile);

            return storedFile.Id;
        }

        private async Task<string> GetBinaryFileName(Guid? fileId)
        {
            if (!fileId.HasValue)
            {
                return null;
            }

            var file = await _binaryObjectManager.GetOrNullAsync(fileId.Value);
            return file?.Description;
        }

        [AbpAuthorize(AppPermissions.Pages_Tickets_Edit)]
        public async Task RemoveFilesFile(EntityDto input)
        {
            var ticket = await _ticketRepository.FirstOrDefaultAsync(input.Id);
            if (ticket == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!ticket.Files.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _binaryObjectManager.DeleteAsync(ticket.Files.Value);
            ticket.Files = null;
        }

    }
}