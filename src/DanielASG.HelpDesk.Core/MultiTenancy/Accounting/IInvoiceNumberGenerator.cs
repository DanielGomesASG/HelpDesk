using System.Threading.Tasks;
using Abp.Dependency;

namespace DanielASG.HelpDesk.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}