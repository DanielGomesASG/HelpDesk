﻿using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Abstractions;

namespace DanielASG.HelpDesk.OpenIddict.Applications
{
    public interface IAbpOpenIdApplicationStore : IOpenIddictApplicationStore<OpenIddictApplicationModel>
    {
        ValueTask<string> GetClientUriAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken = default);

        ValueTask<string> GetLogoUriAsync(OpenIddictApplicationModel application, CancellationToken cancellationToken = default);
    }
}