using System;
using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Domain.Enums;

namespace UltimateRedditBot.Domain.Queue
{
    public class QueueItem
    {
        #region Properties

        public Guid Id { get; set; }

        public PostType PostType { get; set; }

        public Sort Sort { get; set; }

        public SubredditDto SubredditDto { get; set; }

        public int AmountOfPosts { get; set; }

        public string LastUsedPostName { get; set; }

        #endregion
    }
}
