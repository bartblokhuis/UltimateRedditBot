using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.Discord.App.Services.Queue
{
    public interface IDiscordQueueClient : IQueueClient
    {
        public string Group { get; set; }

        public ulong? ChannelId { get; set; }
    }
}
