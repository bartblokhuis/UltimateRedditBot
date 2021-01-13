using System;
using System.Linq;
using UltimateRedditBot.Discord.App.Discord.Constants;

namespace UltimateRedditBot.Discord.App.Discord
{
    public static class DiscordHelper
    {
        public static bool DoesCommandExist(string command)
        {
            return DiscordConstants.Commands.Any(x => x.Equals(command, StringComparison.Ordinal));
        }
    }
}
