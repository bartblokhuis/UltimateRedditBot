using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class GuildSetting : BaseEntity
    {
        #region Properties

        public string Prefix { get; set; }

        public bool AllowNsfw { get; set; }

        public bool IsEnabled { get; set; }

        public Sort Sort { get; set; }

        public int MaxQueueItems { get; set; }

        public int MaxPlaylists { get; set; }

        public int MaxSubscriptions { get; set; }

        public Guild Guild { get; set; }

        public Guid GuildId { get; set; }

        #endregion
    }
}
