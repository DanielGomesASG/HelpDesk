﻿using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DanielASG.HelpDesk.Authorization;
using DanielASG.HelpDesk.Editions;
using DanielASG.HelpDesk.MultiTenancy;
using DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Editions;
using DanielASG.HelpDesk.Web.Controllers;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_Editions)]
    public class EditionsController : HelpDeskControllerBase
    {
        private readonly IEditionAppService _editionAppService;
        private readonly TenantManager _tenantManager;

        public EditionsController(
            IEditionAppService editionAppService, 
            TenantManager tenantManager)
        {
            _editionAppService = editionAppService;
            _tenantManager = tenantManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Editions_Create)]
        public async Task<PartialViewResult> CreateModal(int? id)
        {
            var output = await _editionAppService.GetEditionForEdit(new NullableIdDto { Id = id });
            var viewModel = ObjectMapper.Map<CreateEditionModalViewModel>(output);
            viewModel.EditionItems = await _editionAppService.GetEditionComboboxItems(); ;
            viewModel.FreeEditionItems = await _editionAppService.GetEditionComboboxItems(output.Edition.ExpiringEditionId, false, true); ;
  
            return PartialView("_CreateModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Editions_Create, AppPermissions.Pages_Editions_Edit)]
        public async Task<PartialViewResult> EditModal(int? id)
        {
            var output = await _editionAppService.GetEditionForEdit(new NullableIdDto { Id = id });
            var viewModel = ObjectMapper.Map<EditEditionModalViewModel>(output);
            viewModel.EditionItems = await _editionAppService.GetEditionComboboxItems(); ;
            viewModel.FreeEditionItems = await _editionAppService.GetEditionComboboxItems(output.Edition.ExpiringEditionId, false, true); ;

            return PartialView("_EditModal", viewModel);
        }

        public async Task<PartialViewResult> MoveTenantsToAnotherEdition(int id)
        {
            var editionItems = await _editionAppService.GetEditionComboboxItems();
            var tenantCount = _tenantManager.Tenants.Count(t => t.EditionId == id);

            var viewModel = new MoveTenantsToAnotherEditionViewModel
            {
                EditionId = id,
                TenantCount = tenantCount,
                EditionItems = editionItems
            };

            return PartialView("_MoveTenantsToAnotherEdition", viewModel);
        }
    }
}