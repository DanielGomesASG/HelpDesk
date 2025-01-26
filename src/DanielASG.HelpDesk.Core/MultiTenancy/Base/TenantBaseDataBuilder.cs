using Abp.Dependency;
using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Departments;
using DanielASG.HelpDesk.Tickets;

namespace DanielASG.HelpDesk.MultiTenancy.Base
{
    public class TenantBaseDataBuilder : HelpDeskServiceBase, ITransientDependency
    {
        private readonly IRepository<Status> _statusRepository;
        private readonly IRepository<MessageType> _messageTypeRepository;
        private readonly IRepository<Department> _departmentRepository;

        public TenantBaseDataBuilder(
            IRepository<Status> statusRepository,
            IRepository<MessageType> messageTypeRepository,
            IRepository<Department> departmentRepository
            )
        {
            _statusRepository = statusRepository;
            _messageTypeRepository = messageTypeRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task BuildForAsync(Tenant tenant)
        {
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                await BuildForInternalAsync(tenant);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        private async Task BuildForInternalAsync(Tenant tenant)
        {
            await CreateAndSaveStatus(tenant);
            await CreateAndSaveMessageTypes(tenant);
            await CreateAndSaveDepartments(tenant);
        }

        private async Task CreateAndSaveStatus(Tenant tenant)
        {
            var statuses = new List<Status>
            {
                new Status { Name="Aguardando Atendente",   Color = "#ffffff",    BackgroundColor = "#ffa900",    Code = EStatus.AguardandoAtendente,   Description = "Nenhum atendente vinculado",           IsActive = true },
                new Status { Name="Atendente Vinculado",    Color = "#ffffff",    BackgroundColor = "#159fff",    Code = EStatus.AtendenteVinculado,    Description = "Aguardando resposta do atendente",     IsActive = true },
                new Status { Name="Aberto",                 Color = "#ffffff",    BackgroundColor = "#27ae60",    Code = EStatus.Aberto,                Description = "Conversa iniciada",                    IsActive = true },
                new Status { Name="Finalizado",             Color = "#000000",    BackgroundColor = "#e0e0e0",    Code = EStatus.Finalizado,            Description = "Finalizado",                           IsActive = true },
            };

            foreach (var status in statuses)
            {
                status.TenantId = tenant.Id;

                var exist = await _statusRepository.FirstOrDefaultAsync(a => a.Code == status.Code);
                if (exist == null)
                    await _statusRepository.InsertAsync(status);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        private async Task CreateAndSaveMessageTypes(Tenant tenant)
        {
            var messageTypes = new List<MessageType>
            {
                new MessageType { Name="Suporte Especializado",   Code = EMessageType.SuporteEspecializado,   Description = "Suporte a problemas de alta complexidade",   IsActive = true },
            };

            foreach (var messageType in messageTypes)
            {
                messageType.TenantId = tenant.Id;

                var exist = await _messageTypeRepository.FirstOrDefaultAsync(a => a.Code == messageType.Code);
                if (exist == null)
                    await _messageTypeRepository.InsertAsync(messageType);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        private async Task CreateAndSaveDepartments(Tenant tenant)
        {
            var departments = new List<Department>
            {
                new Department { Name="Entrega.AI",    Code = EDepartment.EntregaAI,     IsActive = true },
                new Department { Name="Solicita.AI",   Code = EDepartment.SolicitaAI,    IsActive = true },
                new Department { Name="Xml.AI",        Code = EDepartment.XMLAI,         IsActive = true },
                new Department { Name="DataPlus.AI",   Code = EDepartment.DataPlusAI,    IsActive = true },
                new Department { Name="Receba.AI",     Code = EDepartment.RecebaAI,      IsActive = true },
                new Department { Name="Reembolsa.AI",  Code = EDepartment.ReembolsaAI,   IsActive = true },
            };

            foreach (var department in departments)
            {
                department.TenantId = tenant.Id;

                var exist = await _departmentRepository.FirstOrDefaultAsync(a => a.Code == department.Code);
                if (exist == null)
                    await _departmentRepository.InsertAsync(department);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }
    }
}
