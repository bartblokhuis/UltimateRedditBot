using Domain.Entities.Common;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        #region Properties

        public string DiscordUserId { get; set; }

        public ICollection<GuildAdmin> GuildAdmins { get; set; }

        #endregion
    }
}
