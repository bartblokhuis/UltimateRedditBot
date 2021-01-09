using System;
using UltimateRedditBot.Domain.Enums;
using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Domain.Models.Reddit
{
    public class Post : BaseEntity<string>, IFullyAuditedObject
    {
        #region Constructor

        public Post()
        { }

        public Post(string postId, string author, int downs, int ups, bool isOver18, string title, string postLink, Uri thumbnail, string selfText, Uri url, PostType postType)
        {
            Id = postId;
            Author = author;
            Downs = downs;
            Ups = ups;
            IsOver18 = isOver18;
            Title = title;
            PostLink = postLink;
            Thumbnail = thumbnail;
            Selftext = selfText;
            Url = url;
            PostType = postType;
        }

        #endregion

        #region Properties

        public string Author { get; set; }

        public int Downs { get; set; }

        public int Ups { get; set; }

        public bool IsOver18 { get; set; }

        public string Title { get; set; }

        public string PostLink { get; set; }

        public Uri Thumbnail { get; set; }

        public string Selftext { get; set; }

        public Uri Url { get; set; }

        public virtual Subreddit Subreddit { get; set; }

        public int SubRedditId { get; set; }

        public PostType PostType { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        #endregion

        #region Methods

        public PostType GetPostType()
        {
            if (Url is null)
                return PostType;

            var url = Url.ToString();

            if (url.Contains(".gif") || url.Contains("https://gfycat") || url.Contains("https://redgifs"))
                PostType = PostType.Gif;

            if (url.Contains(".jpg") || url.Contains(".png") || url.Contains(".jpeg"))
                PostType = PostType.Gif;

            if (url.Contains(".mp4"))
                PostType = PostType.Video;

            return PostType;
        }

        #endregion
    }
}
