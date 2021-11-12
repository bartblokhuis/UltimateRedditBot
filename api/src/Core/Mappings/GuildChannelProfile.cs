using AutoMapper;
using Core.Features.GuidChannels.Commands;
using Domain.Dtos;
using Domain.Entities;

namespace Core.Mappings;

public class GuildChannelProfile : Profile
{
    public GuildChannelProfile()
    {
        CreateMap<GuildChannelDto, GuildChannel>().ReverseMap();
        CreateMap<AddChannelCommand, GuildChannel>().ReverseMap();
    }
}