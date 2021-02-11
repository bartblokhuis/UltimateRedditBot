using System;
using System.Collections.Generic;
using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Discord.Domain.Models
{
    public class User : BaseEntity<ulong>, IFullyAuditedObject
    {
        public virtual ICollection<PostHistory> PostHistories { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public DateTime UpdatedAtUTC { get; set; }
    }
}