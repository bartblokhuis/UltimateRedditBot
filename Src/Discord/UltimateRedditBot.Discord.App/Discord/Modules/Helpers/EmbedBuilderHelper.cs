using System.Collections.Generic;
using System.Linq;
using Discord;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Helpers
{
    public static class EmbedBuilderHelper
    {
        public static List<EmbedFieldBuilder> EmbedQueueItems(IEnumerable<QueueItem> queueItems)
        {
            return queueItems.Select(queueItem => new EmbedFieldBuilder {Name = queueItem.SubredditDto.Name, Value = $"{queueItem.AmountOfPosts}, {(queueItem.AmountOfPosts > 1 ? "times" : "time")} in the queue"}).ToList();
        }

        public static EmbedFooterBuilder QueueItemsFooter(long amountOfItemsInQueue)
            => new EmbedFooterBuilder().WithText($"Total: {amountOfItemsInQueue} requests in the queue");
    }
}
