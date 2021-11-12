using AutoMapper;
using Core.Features.Guilds.Commands;
using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;

namespace Core.Mappings;

public class GuildProfile : Profile
{
    public GuildProfile()
    {
        CreateMap<Guild, GuildDto>().ReverseMap();

        CreateMap<ListResult<Guild>, ListResult<GuildDto>>();

        CreateMap<CreateGuildCommand, Guild>();
    }
}

