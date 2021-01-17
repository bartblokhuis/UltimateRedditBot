using System.Threading.Tasks;

namespace UltimateRedditBot.App.Services.Queue
{
    public interface IQueueService
    {
        Task<string> AddToQueue(string group, ulong id, string subreddit, int amountOfTimes);
    }
}
