using Microsoft.EntityFrameworkCore;
using DanielASG.HelpDesk.OpenIddict.Applications;
using DanielASG.HelpDesk.OpenIddict.Authorizations;
using DanielASG.HelpDesk.OpenIddict.Scopes;
using DanielASG.HelpDesk.OpenIddict.Tokens;

namespace DanielASG.HelpDesk.EntityFrameworkCore
{
    public interface IOpenIddictDbContext
    {
        DbSet<OpenIddictApplication> Applications { get; }

        DbSet<OpenIddictAuthorization> Authorizations { get; }

        DbSet<OpenIddictScope> Scopes { get; }

        DbSet<OpenIddictToken> Tokens { get; }
    }

}