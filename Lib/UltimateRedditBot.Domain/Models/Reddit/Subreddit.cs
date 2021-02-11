using System;
using System.Collections.Generic;
using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Domain.Models.Reddit
{
    public class Subreddit : BaseEntity, IFullyAuditedObject
    {
        #region Methods

        public void Update(string name, bool isNsfw)
        {
            Name = name;
            IsNsfw = isNsfw;
        }

        #endregion

        #region Constructor

        //Empty constructor for ef core
        public Subreddit()
        {
        }

        public Subreddit(string name, bool isNsfw)
        {
            Name = name;
            IsNsfw = isNsfw;
            Posts = new List<Post>();
        }

        #endregion

        #region Properties

        public string Name { get; protected set; }

        public bool IsNsfw { get; protected set; }

        public virtual IEnumerable<Post> Posts { get; set; }

        public DateTime CreatedAtUTC { get; set; }

        public DateTime UpdatedAtUTC { get; set; }

        #endregion
    }
}