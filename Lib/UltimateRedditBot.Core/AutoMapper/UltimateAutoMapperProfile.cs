using AutoMapper;
using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Domain.Models.Reddit;

namespace UltimateRedditBot.Core.AutoMapper
{
    public class UltimateAutoMapperProfile : Profile
    {
        public UltimateAutoMapperProfile()
        {
            CreateMap<Subreddit, SubredditDto>();
            CreateMap<SubredditDto, Subreddit>();
        }
    }
}
