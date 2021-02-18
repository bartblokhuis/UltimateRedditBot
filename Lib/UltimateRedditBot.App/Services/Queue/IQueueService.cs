using System.Collections.Generic;
using System.Threading.Tasks;
using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.App.Services.Queue
{
    public interface IQueueService
    {
        Task<string> AddToQueue<T>(T options, SubredditDto subreddit, int amountOfTimes)
            where T : IAddToQueueOptions;

        IQueueClient GetQueueClient<T>(T identifier);

        string ClearQueue<T>(T identifier);

        Task<string> RemoveFromQueue<T>(T identifier, string subredditName);
    }
}
