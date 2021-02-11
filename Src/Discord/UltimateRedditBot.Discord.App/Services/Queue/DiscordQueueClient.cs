using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.App.Services.Queue;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Discord.App.Services.Queue
{
    public class DiscordQueueClient : QueueClient, IDiscordQueueClient
    {
        public DiscordQueueClient(IRedditApiService redditApiService, IEventPublisher eventPublisher) : base(
            redditApiService, eventPublisher)
        {
        }

        public string Group { get; set; }

        public ulong? ChannelId { get; set; }
    }
}