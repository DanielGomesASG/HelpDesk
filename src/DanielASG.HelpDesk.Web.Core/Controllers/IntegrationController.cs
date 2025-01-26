using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using DanielASG.HelpDesk.Integration;
using DanielASG.HelpDesk.Integration.Dtos;
using DanielASG.HelpDesk.MultiTenancy;
using DanielASG.HelpDesk.Url;

namespace DanielASG.HelpDesk.Web.Controllers
{
    [AllowAnonymous]
    public class IntegrationController : HelpDeskControllerBase
    {
        private readonly IIntegrationAppService _integrationAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly IRepository<Tenant> _tenantRepository;

        protected readonly IWebUrlService _webUrlService;
        public IntegrationController(
            IIntegrationAppService integrationAppService,
            IUnitOfWorkManager unitOfWorkManager,

            IRepository<Tenant> tenantRepository,

            IWebUrlService webUrlService)
        {
            _integrationAppService = integrationAppService;
            _unitOfWorkManager = unitOfWorkManager;

            _tenantRepository = tenantRepository;

            _webUrlService = webUrlService;
        }

        [AllowAnonymous]
        [Route("/oauth/token")]
        [HttpPost]
        public async Task<ActionResult> AccessToken()
        {
            try
            {
                string body;
                using (var reader = new StreamReader(Request.Body))
                {
                    body = await reader.ReadToEndAsync();
                }

                Logger.Warn($"GenerateAccessToken_____________________________________________________________________________");
                Logger.Warn($"Body: {body}\n");

                GenerateAccessTokenInput input;
                try
                {
                    input = JsonConvert.DeserializeObject<GenerateAccessTokenInput>(body);
                }
                catch (Exception ex)
                {
                    Logger.Log(LogSeverity.Error, $"Invalid JSON format: {ex.Message}", ex);
                    throw new Exception($"Invalid JSON format");
                }

                var output = await _integrationAppService.GenerateAccessToken(input);

                var response = JsonConvert.SerializeObject(new { access_token = output.AccessToken });

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, $"Connect: {ex.Message}", ex);
                return StatusCode(500, new { ex.Message });
            }
        }

        [Route("/support/login")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Login()
        {
            try
            {
                string body;
                using (var reader = new StreamReader(Request.Body))
                {
                    body = await reader.ReadToEndAsync();
                }

                Logger.Warn($"LoginCustomer_____________________________________________________________________________");
                Logger.Warn($"Body: {body}\n");

                LoginCustomerUserInput input;
                try
                {
                    input = JsonConvert.DeserializeObject<LoginCustomerUserInput>(body);
                }
                catch (Exception ex)
                {
                    Logger.Log(LogSeverity.Error, $"Invalid JSON format: {ex.Message}", ex);
                    throw new Exception($"Invalid JSON format");
                }

                var accessToken = await _integrationAppService.LoginUser(input);

                var response = JsonConvert.SerializeObject(new
                {
                    access_token = accessToken,
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.Log(LogSeverity.Error, $"LoginCustomer: {ex.Message}", ex);
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
