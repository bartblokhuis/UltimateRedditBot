using System.Threading.Tasks;
using UltimateRedditBot.Domain.Dtos.Reddit;

namespace UltimateRedditBot.Infra.Services
{
    public interface ISubredditService
    {
        Task<SubredditDto> GetSubredditDtoByName(string subredditName);
    }
}