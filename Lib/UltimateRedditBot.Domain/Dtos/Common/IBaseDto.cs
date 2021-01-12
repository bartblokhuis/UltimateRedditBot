using System;
using System.Collections.Generic;
using System.Text;

namespace UltimateRedditBot.Domain.Dtos.Common
{
    public interface IBaseDto<TKEy>
    {
    }

    public interface IBaseDto : IBaseDto<int>
    {
    }
}
