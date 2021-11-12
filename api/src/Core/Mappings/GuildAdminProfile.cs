using AutoMapper;
using Core.Features.GuildAdmins.Commands;
using Domain.Entities;

namespace Core.Mappings;

public class GuildAdminProfile : Profile
{
    public GuildAdminProfile()
    {
        CreateMap<AddGuildAdminCommand, GuildAdmin>().ReverseMap();
    }
}

