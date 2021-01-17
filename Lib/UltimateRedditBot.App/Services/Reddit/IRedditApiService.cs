using System.Threading.Tasks;
using UltimateRedditBot.Domain.Dtos.Reddit;

namespace UltimateRedditBot.App.Services.Reddit
{
    public interface IRedditApiService
    {
        Task<SubredditDto> GetSubredditByName(string subredditName);
    }
}
