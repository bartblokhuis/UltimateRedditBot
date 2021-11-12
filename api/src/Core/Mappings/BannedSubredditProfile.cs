using AutoMapper;
using Core.Features.BannedSubreddits.Commands;
using Domain.Entities;

namespace Core.Mappings;

public class BannedSubredditProfile : Profile
{
    public BannedSubredditProfile()
    {
        CreateMap<BannedSubreddit, BanSubredditCommand>().ReverseMap();
    }
}

