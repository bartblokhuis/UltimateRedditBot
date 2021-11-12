using Domain.Entities.Common;

namespace Domain.Entities
{
    public class Post : BaseEntity
    {
        #region Properties

        public string Author { get; set; }

        public bool IsOver18 { get; set; }

        public string Title { get; set; }

        public string PostLink { get; set; }

        public string Thumbnail { get; set; }

        public string Selftext { get; set; }

        public string Url { get; set; }

        public string PostId { get; set; }

        public string Description { get; set; }

        public string Permalink { get; set; }

        #endregion
    }
}
