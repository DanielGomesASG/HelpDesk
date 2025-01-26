using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DanielASG.HelpDesk.Web.Authentication.JwtBearer
{
    public class AsyncJwtBearerOptions : JwtBearerOptions
    {
        public readonly List<IAsyncSecurityTokenValidator> AsyncSecurityTokenValidators;
        
        private readonly HelpDeskAsyncJwtSecurityTokenHandler _defaultAsyncHandler = new HelpDeskAsyncJwtSecurityTokenHandler();

        public AsyncJwtBearerOptions()
        {
            AsyncSecurityTokenValidators = new List<IAsyncSecurityTokenValidator>() {_defaultAsyncHandler};
        }
    }

}
