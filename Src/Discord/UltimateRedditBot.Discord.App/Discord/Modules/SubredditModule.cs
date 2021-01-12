using System.Threading.Tasks;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;

namespace UltimateRedditBot.Discord.App.Discord.Modules
{
    public class SubredditModule : UltimateCommandModule
    {
        #region Fields



        #endregion

        #region Constructor



        #endregion

        #region Methods

        public async Task GetSubreddits()
        {
            await ReplyAsync("memes, funny");
        }

        public async Task GetSubreddits(int page)
        {

        }

        #endregion
    }
}
