using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.Discord.Domain.Queue
{
    public interface IAddToQueueDiscordOptions : IAddToQueueOptions
    {
        public ulong? ChannelId { get; set; }
    }

    public class AddToQueueDiscordOptions : IAddToQueueDiscordOptions
    {
        public string Group { get; set; }
        public ulong ClientId { get; set; }

        public ulong? ChannelId { get; set; }


    }
}
