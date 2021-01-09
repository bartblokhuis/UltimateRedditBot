using System;
using System.Collections.Generic;
using System.Text;

namespace UltimateRedditBot.Domain.Models.Common
{
    public interface IFullyAuditedObject : IHasCreationDate, IHasUpdatedDate
    {
    }
}
