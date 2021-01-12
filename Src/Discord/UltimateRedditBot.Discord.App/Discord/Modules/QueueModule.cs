using System.Threading.Tasks;
using Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;

namespace UltimateRedditBot.Discord.App.Discord.Modules
{
    public class QueueModule : UltimateCommandModule
    {
        #region Fields



        #endregion

        #region Constructor



        #endregion

        #region Methods

        [Command("r"), Alias("r")]
        public async Task AddToQueue(string subreddit)
        {
            await ReplyAsync("Done sire");
        }

        [Command("r"), Alias("r")]
        public async Task AddToQueue(string subreddit, int amountOfTimes)
        {
            await ReplyAsync($"Done {amountOfTimes} times");
        }

        [Command("r-remove"), Alias("r-remove")]
        public async Task RemoveFromQueue(string subreddit)
        {
            await ReplyAsync("Removed from queue");
        }

        [Command("r-clear"), Alias("r-clear")]
        public async Task ClearCueue()
        {
            await ReplyAsync("Cleared queue");
        }

        #endregion
    }
}
