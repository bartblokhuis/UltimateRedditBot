using System;
using System.Linq;
using UltimateRedditBot.Discord.App.Discord.Constants;

namespace UltimateRedditBot.Discord.App.Discord.Helpers
{
    public static class DiscordHelper
    {
        public static bool DoesCommandExist(string command)
        {
            return DiscordConstants.Commands.Any(commandHelper => commandHelper.Name.Equals(command, StringComparison.OrdinalIgnoreCase));
        }

        public static CommandHelper GetCommandHelperByName(string command)
        {
            return DiscordConstants.Commands.FirstOrDefault(commandHelper => commandHelper.Name.Equals(command, StringComparison.OrdinalIgnoreCase));
        }

        public static int AmountOfArguments(string command)
        {
            var count = 0;
            var wasInWord = false;
            var inWord = false;

            foreach (var t in command)
            {
                if (inWord)
                    wasInWord = true;

                if (char.IsWhiteSpace(t))
                {
                    if (wasInWord)
                    {
                        count++;
                        wasInWord = false;
                    }
                    inWord = false;
                }
                else
                    inWord = true;
            }

            if (wasInWord)
                count++;

            //Remove 1 from the counted words because the command itself is also counted as an arg.
            count--;
            return count;
        }
    }
}
