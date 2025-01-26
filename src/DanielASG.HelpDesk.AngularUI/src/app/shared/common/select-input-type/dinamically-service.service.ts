import { DepartmentsServiceProxy, MessageTypesServiceProxy, StandardMessagesServiceProxy, StatusesServiceProxy, UserServiceProxy } from "../../../../shared/service-proxies/service-proxies";

export const dinamicallyService = {
    //Company: CompaniesServiceProxy,
    //Subsidiary: SubsidiariesServiceProxy,
    //Client: ClientsServiceProxy,
    //ClientType: ClientTypesServiceProxy,
    //ClientSegment: ClientSegmentsServiceProxy,
    //ClientCenterCost: ClientCenterCostsServiceProxy,
    //ClientRegion: ClientRegionsServiceProxy,

    User: UserServiceProxy,
    Department: DepartmentsServiceProxy,
    MessageType: MessageTypesServiceProxy,
    StandardMessage: StandardMessagesServiceProxy,
    Status: StatusesServiceProxy
}
