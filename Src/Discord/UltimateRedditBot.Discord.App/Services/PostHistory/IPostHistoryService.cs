using System.Threading.Tasks;
using UltimateRedditBot.Discord.Domain.Models;

namespace UltimateRedditBot.Discord.App.Services
{
    public interface IPostHistoryService
    {
        PostHistory GetPostHistory(bool isForGuild, ulong id, int subredditId);

        string GetPostHistoryPostId(bool isForGuild, ulong id, int subredditId);

        Task SavePostHistory(PostHistory postHistory);

        Task ClearPostHistory(bool isForGuild, ulong id, int subredditId);
    }
}
