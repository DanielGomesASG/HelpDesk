using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DanielASG.HelpDesk.MultiTenancy.Accounting.Dto;

namespace DanielASG.HelpDesk.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
