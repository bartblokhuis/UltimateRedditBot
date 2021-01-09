using System;
using System.Collections.Generic;
using System.Text;
using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Domain.Models.Reddit
{
    public class Subreddit : BaseEntity, IFullyAuditedObject
    {
        #region Constructor

        //Empty constructor for ef core
        public Subreddit()
        { }

        public SubReddit(string name, bool isNsfw)
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

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        #endregion

        #region Methods

        public void Update(string name, bool isNsfw)
        {
            Name = name;
            IsNsfw = isNsfw;
        }

        #endregion
    }
}
