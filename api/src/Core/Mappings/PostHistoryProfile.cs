using AutoMapper;
using Domain.Entities;

namespace Core.Mappings;

public class PostHistoryProfile : Profile
{
    public PostHistoryProfile()
    {
        CreateMap<PostHistoryDto, PostHistory>().ReverseMap();
    }
}

