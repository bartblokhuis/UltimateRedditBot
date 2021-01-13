using System.Collections.Generic;
using System.Linq;

namespace UltimateRedditBot.Discord.App.Discord.Helpers
{
    public class CommandHelper
    {
        #region Properties

        public string Name { get; init; }

        public IEnumerable<int> AllowedArgs { get; init; }

        #endregion

        #region Methods

        public bool IsValid(string msg)
        {
            var amountOfArgs = DiscordHelper.AmountOfArguments(msg);
            return AllowedArgs.Any(allowedArg => allowedArg == amountOfArgs);
        }

        #endregion
    }
}
