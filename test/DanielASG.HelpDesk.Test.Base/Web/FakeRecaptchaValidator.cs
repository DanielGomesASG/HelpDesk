using System.Threading.Tasks;
using DanielASG.HelpDesk.Security.Recaptcha;

namespace DanielASG.HelpDesk.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
