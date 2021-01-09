using System;
using System.Collections.Generic;
using System.Text;
using UltimateRedditBot.Domain.Models.Common;

namespace UltimateRedditBot.Domain.Models.Reddit
{
    public class GenericSettings : BaseEntity
    {
        #region Constructors

        public GenericSettings()
        { }

        #endregion

        #region Properties

        public string EntityId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        #endregion
    }
}
