using System.Threading.Tasks;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.App.Services.Queue
{
    public interface IQueueService
    {
        Task<string> AddToQueue<T>(T options, string subredditName, int amountOfTimes)
            where T : IAddToQueueOptions;
    }
}