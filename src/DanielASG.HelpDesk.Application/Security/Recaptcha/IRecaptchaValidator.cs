using System.Threading.Tasks;

namespace DanielASG.HelpDesk.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}