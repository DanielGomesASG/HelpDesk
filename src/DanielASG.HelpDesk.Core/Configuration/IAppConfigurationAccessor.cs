using Microsoft.Extensions.Configuration;

namespace DanielASG.HelpDesk.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
