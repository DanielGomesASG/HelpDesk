﻿using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DanielASG.HelpDesk.Authorization;
using DanielASG.HelpDesk.DynamicEntityProperties;
using DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.DynamicEntityProperty;
using DanielASG.HelpDesk.Web.Controllers;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_DynamicEntityProperties)]
    public class DynamicEntityPropertyController : HelpDeskControllerBase
    {
        private readonly IDynamicPropertyAppService _dynamicPropertyAppService;
        private readonly IDynamicEntityPropertyAppService _dynamicEntityPropertyAppService;

        public DynamicEntityPropertyController(
            IDynamicPropertyAppService dynamicPropertyAppService,
            IDynamicEntityPropertyAppService dynamicEntityPropertyAppService
        )
        {
            _dynamicPropertyAppService = dynamicPropertyAppService;
            _dynamicEntityPropertyAppService = dynamicEntityPropertyAppService;
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_DynamicEntityProperties_Create)]
        public async Task<IActionResult> CreateModal(string entityFullName)
        {
            var model = new CreateEntityDynamicPropertyViewModel()
            {
                EntityFullName = entityFullName
            };

            var allDynamicProperties = (await _dynamicPropertyAppService.GetAll()).Items.ToList();
            var definedPropertyIds = (await _dynamicEntityPropertyAppService.GetAllPropertiesOfAnEntity(new DynamicEntityPropertyGetAllInput() {EntityFullName = entityFullName}))
                .Items.Select(x => x.DynamicPropertyId).ToList();

            model.DynamicProperties = allDynamicProperties.Where(x => !definedPropertyIds.Contains(x.Id)).ToList();

            return PartialView("_CreateModal", model);
        }
        
        public IActionResult ManageModal()
        {
            return PartialView("_ManageModal");
        }
    }
}