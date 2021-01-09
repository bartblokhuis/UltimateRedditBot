using System;

namespace UltimateRedditBot.Domain.Models.Common
{
    public interface IHasCreationDate
    {
        public DateTime CreatedAt { get; set; }
    }
}
