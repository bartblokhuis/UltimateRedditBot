using System;
using System.Threading.Tasks;
using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Domain.Enums;

namespace UltimateRedditBot.Infra.Services
{
    public interface IRedditApiService
    {
        Task<SubredditDto> GetSubredditByName(string subredditName);

        Task<PostDto> GetOldPost(string subRedditName, string previousName, Sort sort, PostType postType, Guid id);

        Task<PostDto> GetSubscriptionPost(string subredditName, string previousName, Sort sort, PostType postType,
            Guid id);

        Task<bool> IsPostRemoved(string postUrl);
    }
}
