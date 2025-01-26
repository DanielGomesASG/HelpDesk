using System.Globalization;

namespace DanielASG.HelpDesk.Localization
{
    public interface IApplicationCulturesProvider
    {
        CultureInfo[] GetAllCultures();
    }
}