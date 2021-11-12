using Domain.Entities.Common;

namespace Domain.Entities
{
    public class GuildAdmin : BaseEntity
    {
        #region Properties

        public Guild Guild { get; set; }

        public Guid GuildId { get; set; }

        public User User { get; set; }

        public Guid UserId { get; set; }

        #endregion
    }
}
