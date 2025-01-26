using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace DanielASG.HelpDesk.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var specialPermissions = context.GetPermissionOrNull(AppPermissions.SpecialPermissions) ?? context.CreatePermission(AppPermissions.SpecialPermissions, L("SpecialPermissions"));
            specialPermissions.CreateChildPermission(AppPermissions.SpecialPermissions_Profiles_Customer, L("CustomerProfile"), multiTenancySides: MultiTenancySides.Tenant);
            specialPermissions.CreateChildPermission(AppPermissions.SpecialPermissions_Profiles_Staff, L("StaffProfile"), multiTenancySides: MultiTenancySides.Tenant);

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            pages.CreateChildPermission(AppPermissions.Pages_Home, L("Home"), multiTenancySides: MultiTenancySides.Tenant);

            var notifications = pages.CreateChildPermission(AppPermissions.Pages_Notifications, L("Notifications"), multiTenancySides: MultiTenancySides.Tenant);

            var tickets = pages.CreateChildPermission(AppPermissions.Pages_Tickets, L("Tickets"), multiTenancySides: MultiTenancySides.Tenant);
            tickets.CreateChildPermission(AppPermissions.Pages_Tickets_Create, L("CreateNewTicket"), multiTenancySides: MultiTenancySides.Tenant);
            tickets.CreateChildPermission(AppPermissions.Pages_Tickets_Edit, L("EditTicket"), multiTenancySides: MultiTenancySides.Tenant);
            tickets.CreateChildPermission(AppPermissions.Pages_Tickets_Bind, L("BindTicket"), multiTenancySides: MultiTenancySides.Tenant);
            tickets.CreateChildPermission(AppPermissions.Pages_Tickets_Delete, L("DeleteTicket"), multiTenancySides: MultiTenancySides.Tenant);
            tickets.CreateChildPermission(AppPermissions.Pages_Tickets_View, L("ViewTicket"), multiTenancySides: MultiTenancySides.Tenant);
            tickets.CreateChildPermission(AppPermissions.Pages_Tickets_All, L("AllTickets"), multiTenancySides: MultiTenancySides.Tenant);

            var ticketDetails = tickets.CreateChildPermission(AppPermissions.Pages_Ticket_Details, L("TicketDetails"), multiTenancySides: MultiTenancySides.Tenant);
            ticketDetails.CreateChildPermission(AppPermissions.Pages_Ticket_Details_RefreshButton, L("RefreshButton"), multiTenancySides: MultiTenancySides.Tenant);

            var departments = pages.CreateChildPermission(AppPermissions.Pages_Departments, L("Departments"), multiTenancySides: MultiTenancySides.Tenant);
            departments.CreateChildPermission(AppPermissions.Pages_Departments_Create, L("CreateNewDepartment"), multiTenancySides: MultiTenancySides.Tenant);
            departments.CreateChildPermission(AppPermissions.Pages_Departments_Edit, L("EditDepartment"), multiTenancySides: MultiTenancySides.Tenant);
            departments.CreateChildPermission(AppPermissions.Pages_Departments_Delete, L("DeleteDepartment"), multiTenancySides: MultiTenancySides.Tenant);

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var registers = pages.CreateChildPermission(AppPermissions.Pages_Register, L("Registers"));

            var contents = registers.CreateChildPermission(AppPermissions.Pages_Administration_Contents, L("Contents"), multiTenancySides: MultiTenancySides.Tenant);
            contents.CreateChildPermission(AppPermissions.Pages_Administration_Contents_Create, L("CreateNewContent"), multiTenancySides: MultiTenancySides.Tenant);
            contents.CreateChildPermission(AppPermissions.Pages_Administration_Contents_Edit, L("EditContent"), multiTenancySides: MultiTenancySides.Tenant);
            contents.CreateChildPermission(AppPermissions.Pages_Administration_Contents_Delete, L("DeleteContent"), multiTenancySides: MultiTenancySides.Tenant);

            var registerTickets = registers.CreateChildPermission(AppPermissions.Pages_Register_Tickets, L("Tickets"), multiTenancySides: MultiTenancySides.Tenant);

            var statuses = registerTickets.CreateChildPermission(AppPermissions.Pages_Register_Tickets_Statuses, L("Statuses"), multiTenancySides: MultiTenancySides.Tenant);
            statuses.CreateChildPermission(AppPermissions.Pages_Register_Tickets_Statuses_Create, L("CreateNewStatus"), multiTenancySides: MultiTenancySides.Tenant);
            statuses.CreateChildPermission(AppPermissions.Pages_Register_Tickets_Statuses_Edit, L("EditStatus"), multiTenancySides: MultiTenancySides.Tenant);
            statuses.CreateChildPermission(AppPermissions.Pages_Register_Tickets_Statuses_Delete, L("DeleteStatus"), multiTenancySides: MultiTenancySides.Tenant);

            var messageTypes = registerTickets.CreateChildPermission(AppPermissions.Pages_Register_Tickets_MessageTypes, L("MessageTypes"), multiTenancySides: MultiTenancySides.Tenant);
            messageTypes.CreateChildPermission(AppPermissions.Pages_Register_Tickets_MessageTypes_Create, L("CreateNewMessageType"), multiTenancySides: MultiTenancySides.Tenant);
            messageTypes.CreateChildPermission(AppPermissions.Pages_Register_Tickets_MessageTypes_Edit, L("EditMessageType"), multiTenancySides: MultiTenancySides.Tenant);
            messageTypes.CreateChildPermission(AppPermissions.Pages_Register_Tickets_MessageTypes_Delete, L("DeleteMessageType"), multiTenancySides: MultiTenancySides.Tenant);

            var standardMessages = registerTickets.CreateChildPermission(AppPermissions.Pages_Register_Tickets_StandardMessages, L("StandardMessages"), multiTenancySides: MultiTenancySides.Tenant);
            standardMessages.CreateChildPermission(AppPermissions.Pages_Register_Tickets_StandardMessages_Create, L("CreateNewStandardMessage"), multiTenancySides: MultiTenancySides.Tenant);
            standardMessages.CreateChildPermission(AppPermissions.Pages_Register_Tickets_StandardMessages_Edit, L("EditStandardMessage"), multiTenancySides: MultiTenancySides.Tenant);
            standardMessages.CreateChildPermission(AppPermissions.Pages_Register_Tickets_StandardMessages_Delete, L("DeleteStandardMessage"), multiTenancySides: MultiTenancySides.Tenant);

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangeProfilePicture, L("UpdateUsersProfilePicture"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_PasswordlessToken, L("GeneratePasswordlessToken"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("ChangeDefaultLanguage"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            var massNotification = administration.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification, L("MassNotifications"));
            massNotification.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification_Create, L("MassNotificationCreate"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);

            var maintenance = administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            maintenance.CreateChildPermission(AppPermissions.Pages_Administration_NewVersion_Create, L("SendNewVersionNotification"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, HelpDeskConsts.LocalizationSourceName);
        }
    }
}