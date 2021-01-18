using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.Discord.Domain.Queue
{
    public interface IDiscordQueueClient : IQueueClient
    {
        public string Group { get; set; }
    }
}
