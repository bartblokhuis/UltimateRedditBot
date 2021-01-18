using System.Collections.Generic;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.Discord.Domain.Queue
{
    public class DiscordQueueClient : IDiscordQueueClient
    {
        public ulong ClientId { get; set; }

        public IEnumerable<QueueItem> QueueItems { get; set; }

        public bool HasQueueItems { get; }
        public string Group { get; set; }
    }
}
