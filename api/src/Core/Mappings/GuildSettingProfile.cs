using AutoMapper;
using Core.Features.Guilds.Commands;
using Domain.Dtos;
using Domain.Entities;

namespace Core.Mappings;

public class GuildSettingProfile : Profile
{
    public GuildSettingProfile()
    {
        CreateMap<GuildSetting, GuildSettingDto>();
        CreateMap<UpdateGuildSettingsCommand, GuildSetting>();
    }
}