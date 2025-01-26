namespace DanielASG.HelpDesk.Authorization
{
    /// <summary>
    /// Defines string constants for application's permission names.
    /// <see cref="AppAuthorizationProvider"/> for permission definitions.
    /// </summary>
    public static class AppPermissions
    {
        //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

        public const string SpecialPermissions = "SpecialPermissions";
        public const string SpecialPermissions_Profiles_Customer = "SpecialPermissions.Profiles.Customer";
        public const string SpecialPermissions_Profiles_Staff = "SpecialPermissions.Profiles.Staff";

        public const string Pages = "Pages";

        public const string Pages_Home = "Pages.Tenant.Home";

        public const string Pages_Notifications = "Pages.Notifications";

        public const string Pages_Tickets = "Pages.Tickets";
        public const string Pages_Tickets_Create = "Pages.Tickets.Create";
        public const string Pages_Tickets_Edit = "Pages.Tickets.Edit";
        public const string Pages_Tickets_Bind = "Pages.Tickets.Bind";
        public const string Pages_Tickets_Delete = "Pages.Tickets.Delete";
        public const string Pages_Tickets_View = "Pages.Tickets.View";
        public const string Pages_Tickets_All = "Pages.Tickets.All";

        public const string Pages_Ticket_Details = "Pages.Ticket.Details";
        public const string Pages_Ticket_Details_RefreshButton = "Pages.Ticket.Details.RefreshButton";

        public const string Pages_Departments = "Pages.Departments";
        public const string Pages_Departments_Create = "Pages.Departments.Create";
        public const string Pages_Departments_Edit = "Pages.Departments.Edit";
        public const string Pages_Departments_Delete = "Pages.Departments.Delete";

        public const string Pages_DemoUiComponents = "Pages.DemoUiComponents";

        public const string Pages_Register = "Pages.Register";
        public const string Pages_Register_Tickets = "Pages.Register.Tickets";
        public const string Pages_Register_Tickets_Statuses = "Pages.Register.Tickets.Statuses";
        public const string Pages_Register_Tickets_Statuses_Create = "Pages.Register.Tickets.Statuses.Create";
        public const string Pages_Register_Tickets_Statuses_Edit = "Pages.Register.Tickets.Statuses.Edit";
        public const string Pages_Register_Tickets_Statuses_Delete = "Pages.Register.Tickets.Statuses.Delete";

        public const string Pages_Register_Tickets_MessageTypes = "Pages.Register.Tickets.MessageTypes";
        public const string Pages_Register_Tickets_MessageTypes_Create = "Pages.Register.Tickets.MessageTypes.Create";
        public const string Pages_Register_Tickets_MessageTypes_Edit = "Pages.Register.Tickets.MessageTypes.Edit";
        public const string Pages_Register_Tickets_MessageTypes_Delete = "Pages.Register.Tickets.MessageTypes.Delete";

        public const string Pages_Register_Tickets_StandardMessages = "Pages.Register.Tickets.StandardMessages";
        public const string Pages_Register_Tickets_StandardMessages_Create = "Pages.Register.Tickets.StandardMessages.Create";
        public const string Pages_Register_Tickets_StandardMessages_Edit = "Pages.Register.Tickets.StandardMessages.Edit";
        public const string Pages_Register_Tickets_StandardMessages_Delete = "Pages.Register.Tickets.StandardMessages.Delete";

        public const string Pages_Administration = "Pages.Administration";

        public const string Pages_Administration_Contents = "Pages.Administration.Contents";
        public const string Pages_Administration_Contents_Create = "Pages.Administration.Contents.Create";
        public const string Pages_Administration_Contents_Edit = "Pages.Administration.Contents.Edit";
        public const string Pages_Administration_Contents_Delete = "Pages.Administration.Contents.Delete";

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_ChangePermissions = "Pages.Administration.Users.ChangePermissions";
        public const string Pages_Administration_Users_Impersonation = "Pages.Administration.Users.Impersonation";
        public const string Pages_Administration_Users_Unlock = "Pages.Administration.Users.Unlock";
        public const string Pages_Administration_Users_ChangeProfilePicture = "Pages.Administration.Users.ChangeProfilePicture";
        public const string Pages_Administration_Users_PasswordlessToken = "Pages.Administration.Users.PasswordlessToken";

        public const string Pages_Administration_Languages = "Pages.Administration.Languages";
        public const string Pages_Administration_Languages_Create = "Pages.Administration.Languages.Create";
        public const string Pages_Administration_Languages_Edit = "Pages.Administration.Languages.Edit";
        public const string Pages_Administration_Languages_Delete = "Pages.Administration.Languages.Delete";
        public const string Pages_Administration_Languages_ChangeTexts = "Pages.Administration.Languages.ChangeTexts";
        public const string Pages_Administration_Languages_ChangeDefaultLanguage = "Pages.Administration.Languages.ChangeDefaultLanguage";

        public const string Pages_Administration_AuditLogs = "Pages.Administration.AuditLogs";

        public const string Pages_Administration_OrganizationUnits = "Pages.Administration.OrganizationUnits";
        public const string Pages_Administration_OrganizationUnits_ManageOrganizationTree = "Pages.Administration.OrganizationUnits.ManageOrganizationTree";
        public const string Pages_Administration_OrganizationUnits_ManageMembers = "Pages.Administration.OrganizationUnits.ManageMembers";
        public const string Pages_Administration_OrganizationUnits_ManageRoles = "Pages.Administration.OrganizationUnits.ManageRoles";

        public const string Pages_Administration_HangfireDashboard = "Pages.Administration.HangfireDashboard";

        public const string Pages_Administration_UiCustomization = "Pages.Administration.UiCustomization";

        public const string Pages_Administration_WebhookSubscription = "Pages.Administration.WebhookSubscription";
        public const string Pages_Administration_WebhookSubscription_Create = "Pages.Administration.WebhookSubscription.Create";
        public const string Pages_Administration_WebhookSubscription_Edit = "Pages.Administration.WebhookSubscription.Edit";
        public const string Pages_Administration_WebhookSubscription_ChangeActivity = "Pages.Administration.WebhookSubscription.ChangeActivity";
        public const string Pages_Administration_WebhookSubscription_Detail = "Pages.Administration.WebhookSubscription.Detail";
        public const string Pages_Administration_Webhook_ListSendAttempts = "Pages.Administration.Webhook.ListSendAttempts";
        public const string Pages_Administration_Webhook_ResendWebhook = "Pages.Administration.Webhook.ResendWebhook";

        public const string Pages_Administration_DynamicProperties = "Pages.Administration.DynamicProperties";
        public const string Pages_Administration_DynamicProperties_Create = "Pages.Administration.DynamicProperties.Create";
        public const string Pages_Administration_DynamicProperties_Edit = "Pages.Administration.DynamicProperties.Edit";
        public const string Pages_Administration_DynamicProperties_Delete = "Pages.Administration.DynamicProperties.Delete";

        public const string Pages_Administration_DynamicPropertyValue = "Pages.Administration.DynamicPropertyValue";
        public const string Pages_Administration_DynamicPropertyValue_Create = "Pages.Administration.DynamicPropertyValue.Create";
        public const string Pages_Administration_DynamicPropertyValue_Edit = "Pages.Administration.DynamicPropertyValue.Edit";
        public const string Pages_Administration_DynamicPropertyValue_Delete = "Pages.Administration.DynamicPropertyValue.Delete";

        public const string Pages_Administration_DynamicEntityProperties = "Pages.Administration.DynamicEntityProperties";
        public const string Pages_Administration_DynamicEntityProperties_Create = "Pages.Administration.DynamicEntityProperties.Create";
        public const string Pages_Administration_DynamicEntityProperties_Edit = "Pages.Administration.DynamicEntityProperties.Edit";
        public const string Pages_Administration_DynamicEntityProperties_Delete = "Pages.Administration.DynamicEntityProperties.Delete";

        public const string Pages_Administration_DynamicEntityPropertyValue = "Pages.Administration.DynamicEntityPropertyValue";
        public const string Pages_Administration_DynamicEntityPropertyValue_Create = "Pages.Administration.DynamicEntityPropertyValue.Create";
        public const string Pages_Administration_DynamicEntityPropertyValue_Edit = "Pages.Administration.DynamicEntityPropertyValue.Edit";
        public const string Pages_Administration_DynamicEntityPropertyValue_Delete = "Pages.Administration.DynamicEntityPropertyValue.Delete";

        public const string Pages_Administration_MassNotification = "Pages.Administration.MassNotification";
        public const string Pages_Administration_MassNotification_Create = "Pages.Administration.MassNotification.Create";

        public const string Pages_Administration_NewVersion_Create = "Pages_Administration_NewVersion_Create";

        //TENANT-SPECIFIC PERMISSIONS

        public const string Pages_Tenant_Dashboard = "Pages.Tenant.Dashboard";

        public const string Pages_Administration_Tenant_Settings = "Pages.Administration.Tenant.Settings";

        public const string Pages_Administration_Tenant_SubscriptionManagement = "Pages.Administration.Tenant.SubscriptionManagement";

        //HOST-SPECIFIC PERMISSIONS

        public const string Pages_Editions = "Pages.Editions";
        public const string Pages_Editions_Create = "Pages.Editions.Create";
        public const string Pages_Editions_Edit = "Pages.Editions.Edit";
        public const string Pages_Editions_Delete = "Pages.Editions.Delete";
        public const string Pages_Editions_MoveTenantsToAnotherEdition = "Pages.Editions.MoveTenantsToAnotherEdition";

        public const string Pages_Tenants = "Pages.Tenants";
        public const string Pages_Tenants_Create = "Pages.Tenants.Create";
        public const string Pages_Tenants_Edit = "Pages.Tenants.Edit";
        public const string Pages_Tenants_ChangeFeatures = "Pages.Tenants.ChangeFeatures";
        public const string Pages_Tenants_Delete = "Pages.Tenants.Delete";
        public const string Pages_Tenants_Impersonation = "Pages.Tenants.Impersonation";

        public const string Pages_Administration_Host_Maintenance = "Pages.Administration.Host.Maintenance";
        public const string Pages_Administration_Host_Settings = "Pages.Administration.Host.Settings";
        public const string Pages_Administration_Host_Dashboard = "Pages.Administration.Host.Dashboard";
    }
}