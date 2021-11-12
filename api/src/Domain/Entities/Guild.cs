using Domain.Entities.Common;

namespace Domain.Entities
{
    public class Guild : BaseEntity
    {
        #region Properties

        public string DiscordGuildId { get; set; }

        public GuildSetting GuildSetting { get; set; }

        public Guid GuildSettingId { get; set; }

        public ICollection<GuildChannel> GuildChannels { get; set; }

        public ICollection<GuildAdmin> GuildAdmins { get; set; }

        public ICollection<BannedSubreddit> BannedSubreddits { get; set; }

        public ICollection<Playlist> GuildPlaylists { get; set; }

        public ICollection<PostHistory> PostHistories {  get; set; }

        #endregion

        #region Constructor

        public Guild()
        {
            GuildChannels = new HashSet<GuildChannel>();
            GuildAdmins = new HashSet<GuildAdmin>();
            BannedSubreddits = new HashSet<BannedSubreddit>();
            GuildPlaylists = new HashSet<Playlist>();
            PostHistories = new HashSet<PostHistory>();
        }

        #endregion
    }
}
