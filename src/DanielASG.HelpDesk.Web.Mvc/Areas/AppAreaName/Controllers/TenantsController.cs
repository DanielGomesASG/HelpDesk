﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DanielASG.HelpDesk.Authorization;
using DanielASG.HelpDesk.Common;
using DanielASG.HelpDesk.Editions;
using DanielASG.HelpDesk.MultiTenancy;
using DanielASG.HelpDesk.Security;
using DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Tenants;
using DanielASG.HelpDesk.Web.Controllers;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_Tenants)]
    public class TenantsController : HelpDeskControllerBase
    {
        private readonly ITenantAppService _tenantAppService;
        private readonly ICommonLookupAppService _commonLookupAppService;
        private readonly TenantManager _tenantManager;
        private readonly IEditionAppService _editionAppService;
        private readonly IPasswordComplexitySettingStore _passwordComplexitySettingStore;

        public TenantsController(
            ITenantAppService tenantAppService,
            TenantManager tenantManager,
            IEditionAppService editionAppService,
            ICommonLookupAppService commonLookupAppService,
            IPasswordComplexitySettingStore passwordComplexitySettingStore)
        {
            _tenantAppService = tenantAppService;
            _tenantManager = tenantManager;
            _editionAppService = editionAppService;
            _commonLookupAppService = commonLookupAppService;
            _passwordComplexitySettingStore = passwordComplexitySettingStore;
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.FilterText = Request.Query["filterText"];
            ViewBag.Sorting = Request.Query["sorting"];
            ViewBag.SubscriptionEndDateStart = Request.Query["subscriptionEndDateStart"];
            ViewBag.SubscriptionEndDateEnd = Request.Query["subscriptionEndDateEnd"];
            ViewBag.CreationDateStart = Request.Query["creationDateStart"];
            ViewBag.CreationDateEnd = Request.Query["creationDateEnd"];
            ViewBag.EditionId = Request.Query.ContainsKey("editionId") ? Convert.ToInt32(Request.Query["editionId"]) : (int?)null;

            return View(new TenantIndexViewModel
            {
                EditionItems = await _editionAppService.GetEditionComboboxItems(selectedEditionId: ViewBag.EditionId, addAllItem: true)
            });
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenants_Create)]
        public async Task<PartialViewResult> CreateModal()
        {
            var editionItems = await _editionAppService.GetEditionComboboxItems();
            var defaultEditionName = _commonLookupAppService.GetDefaultEditionName().Name;
            var defaultEditionItem = editionItems.FirstOrDefault(e => e.DisplayText == defaultEditionName);
            if (defaultEditionItem != null)
            {
                defaultEditionItem.IsSelected = true;
            }

            var viewModel = new CreateTenantViewModel(editionItems)
            {
                PasswordComplexitySetting = await _passwordComplexitySettingStore.GetSettingsAsync()
            };

            return PartialView("_CreateModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenants_Edit)]
        public async Task<PartialViewResult> EditModal(int id)
        {
            var tenantEditDto = await _tenantAppService.GetTenantForEdit(new EntityDto(id));
            var editionItems = await _editionAppService.GetEditionComboboxItems(tenantEditDto.EditionId);
            var viewModel = new EditTenantViewModel(tenantEditDto, editionItems);

            return PartialView("_EditModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenants_ChangeFeatures)]
        public async Task<PartialViewResult> FeaturesModal(int id)
        {
            var output = await _tenantAppService.GetTenantFeaturesForEdit(new EntityDto(id));
            var viewModel = ObjectMapper.Map<TenantFeaturesEditViewModel>(output);
            viewModel.Tenant = await _tenantManager.GetByIdAsync(id);

            return PartialView("_FeaturesModal", viewModel);
        }
    }
}