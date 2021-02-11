using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Domain.Models.Settings
{
    public class GenericSetting : BaseEntity
    {
        #region Constructors

        #endregion

        #region Properties

        public string EntityId { get; set; }

        public string KeyGroup { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        #endregion
    }
}