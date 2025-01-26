using System.Threading.Tasks;

namespace DanielASG.HelpDesk.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
