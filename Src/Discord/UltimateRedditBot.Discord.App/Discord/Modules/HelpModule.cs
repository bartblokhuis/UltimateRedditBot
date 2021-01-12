using System.Threading.Tasks;
using Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;

namespace UltimateRedditBot.Discord.App.Discord.Modules
{
    public class HelpModule : UltimateCommandModule
    {
        #region Fields



        #endregion

        #region Constructor

        public HelpModule()
        {

        }

        #endregion

        #region Methods

        [Command("help"), Alias("help")]
        public async Task Help()
        {
            await ReplyAsync("yes sir");
        }

        #endregion
    }
}
