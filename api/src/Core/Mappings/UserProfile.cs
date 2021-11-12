using AutoMapper;
using Core.Features.Users.Commands;
using Domain.Dtos;
using Domain.Entities;

namespace Core.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, AddUserCommand>().ReverseMap();
    }
}
