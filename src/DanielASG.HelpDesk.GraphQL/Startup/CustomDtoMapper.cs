using AutoMapper;
using DanielASG.HelpDesk.Authorization.Users;
using DanielASG.HelpDesk.Dto;

namespace DanielASG.HelpDesk.Startup
{
    public static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<User, UserDto>()
                .ForMember(dto => dto.Roles, options => options.Ignore())
                .ForMember(dto => dto.OrganizationUnits, options => options.Ignore());
        }
    }
}