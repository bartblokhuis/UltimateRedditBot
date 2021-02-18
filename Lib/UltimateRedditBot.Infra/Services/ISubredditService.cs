using System.Collections.Generic;
using System.Threading.Tasks;
using UltimateRedditBot.Domain.Dtos.Reddit;

namespace UltimateRedditBot.Infra.Services
{
    public interface ISubredditService
    {
        Task<SubredditDto> GetSubredditDtoByName(string subredditName);

        Task<SubredditDto> GetRandomSubreddit(bool isOver18, IEnumerable<int> bannedSubredditIds = null);

        Task<IEnumerable<SubredditDto>> GetAllByIds(IEnumerable<int> ids);
    }
}
