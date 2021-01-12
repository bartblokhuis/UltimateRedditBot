using System;
using System.Collections.Generic;
using System.Text;
using UltimateRedditBot.Domain.Dtos.Common;
using UltimateRedditBot.Domain.Models.Reddit;

namespace UltimateRedditBot.Domain.Dtos.Reddit
{
    public class SubredditDto : BaseDto
    {
        #region Constructors

        public SubredditDto(string name, bool isNsfw)
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
    }
}
