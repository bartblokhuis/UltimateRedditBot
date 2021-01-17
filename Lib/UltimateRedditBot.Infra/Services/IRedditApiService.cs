using System.Threading.Tasks;
using UltimateRedditBot.Domain.Dtos.Reddit;

namespace UltimateRedditBot.Infra.Services
{
    public interface IRedditApiService
    {
        Task<SubredditDto> GetSubredditByName(string subredditName);
    }
}
