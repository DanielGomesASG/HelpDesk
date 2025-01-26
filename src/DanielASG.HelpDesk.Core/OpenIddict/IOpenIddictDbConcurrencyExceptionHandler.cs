using System.Threading.Tasks;
using Abp.Domain.Uow;

namespace DanielASG.HelpDesk.OpenIddict
{
    public interface IOpenIddictDbConcurrencyExceptionHandler
    {
        Task HandleAsync(AbpDbConcurrencyException exception);
    }
}