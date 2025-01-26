using System.Collections.Generic;
using DanielASG.HelpDesk.Editions.Dto;
using DanielASG.HelpDesk.MultiTenancy.Dto;

namespace DanielASG.HelpDesk.Web.Areas.AppAreaName.Models.Tenants
{
    public class EditTenantViewModel
    {
        public TenantEditDto Tenant { get; set; }

        public IReadOnlyList<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public EditTenantViewModel(TenantEditDto tenant, IReadOnlyList<SubscribableEditionComboboxItemDto> editionItems)
        {
            Tenant = tenant;
            EditionItems = editionItems;
        }
    }
}