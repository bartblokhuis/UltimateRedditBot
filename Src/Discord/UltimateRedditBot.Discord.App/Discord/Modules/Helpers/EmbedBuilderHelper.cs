using System.Collections.Generic;
using Discord;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Helpers
{
    public static class EmbedBuilderHelper
    {
        public static List<EmbedFieldBuilder> EmbedQueueItems(IEnumerable<QueueItem> queueItems)
        {
            var embedBuilder = new List<EmbedFieldBuilder>();

            foreach (var queueItem in queueItems)
            {
                embedBuilder.Add(new EmbedFieldBuilder
                {
                    Name = queueItem.SubredditDto.Name,
                    Value = $"{queueItem.AmountOfPosts}, {(queueItem.AmountOfPosts > 1 ? "times" : "time")} in the queue"
                });
            }

            return embedBuilder;
        }

        public static EmbedFooterBuilder QueueItemsFooter(long amountOfItemsInQueue)
            => new EmbedFooterBuilder().WithText($"Total: {amountOfItemsInQueue} requests in the queue");
    }
}
