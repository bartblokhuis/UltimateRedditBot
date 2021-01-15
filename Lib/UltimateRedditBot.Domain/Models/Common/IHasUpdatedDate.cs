using System;

namespace UltimateRedditBot.Domain.Models.Common
{
    public interface IHasUpdatedDate
    {
        public DateTime CreatedAtUTC { get; set; }
    }
}
