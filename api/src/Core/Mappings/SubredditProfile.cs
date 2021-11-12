using AutoMapper;
using Core.Features.Subreddits.Commands;
using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;

namespace Core.Mappings;

public class SubredditProfile : Profile
{
    public SubredditProfile()
    {
        CreateMap<SubredditDto, Subreddit>().ReverseMap();
        CreateMap<ListResult<Subreddit>, ListResult<SubredditDto>>();
        CreateMap<AddSubredditCommand, Subreddit>().ReverseMap();
    }
}

