using System.Collections.Generic;
using UltimateRedditBot.Discord.App.Discord.Helpers;

namespace UltimateRedditBot.Discord.App.Discord.Constants
{
    public static class DiscordConstants
    {
        public static readonly string[] Commands_ = new string[]
        {
            "help",
            "r"
        };

        public static readonly CommandHelper[] Commands =
        {
            new()
            {
                Name = "Help",
                AllowedArgs = new List<int>{0}
            },
            new()
            {
                Name = "R",
                AllowedArgs = new List<int>{0,1,2}
            }
        };
    }
}
