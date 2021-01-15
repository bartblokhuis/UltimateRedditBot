using System.Threading.Tasks;
using Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;

namespace UltimateRedditBot.Discord.App.Discord.Modules.DirectMessage
{
    public class DirectMessagesQueueModule : UltimateDirectMessageModule
    {
        #region Fields



        #endregion

        #region Constructor



        #endregion

        #region Methods

        [Command("r"), Alias("r")]
        public async Task AddToQueue(string subreddit)
        {
        }

        [Command("r"), Alias("r")]
        public async Task AddToQueue(string subreddit, int amountOfTimes)
        {
        }

        [Command("r-remove"), Alias("r-remove")]
        public async Task RemoveFromQueue(string subreddit)
        {
        }

        [Command("r-clear"), Alias("r-clear")]
        public async Task ClearQueue()
        {
        }

        #endregion
    }
}
