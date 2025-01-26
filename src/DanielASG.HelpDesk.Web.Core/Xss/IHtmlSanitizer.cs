using Abp.Dependency;

namespace DanielASG.HelpDesk.Web.Xss
{
    public interface IHtmlSanitizer: ITransientDependency
    {
        string Sanitize(string html);
    }
}