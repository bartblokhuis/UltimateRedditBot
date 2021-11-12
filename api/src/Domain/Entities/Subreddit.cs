using Domain.Entities.Common;

namespace Domain.Entities
{
    public class Subreddit : BaseEntity
    {
        #region Properties

        public string Name { get; set; }

        public bool IsNsfw { get; set; }

        public bool IsBanned { get; set; }

        public ICollection<Playlist> Playlists{ get; set; }

        #endregion

        #region Constructor

        public Subreddit()
        {
            Playlists = new HashSet<Playlist>();
        }

        #endregion
    }
}
