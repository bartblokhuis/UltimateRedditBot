using System.Threading.Tasks;
using Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;

namespace UltimateRedditBot.Discord.App.Discord.Modules.DirectMessage
{
    public class DirectMessageSettingsModule : UltimateDirectMessageModule
    {
        #region Fields



        #endregion

        #region Constructor

        public DirectMessageSettingsModule()
        {

        }

        #endregion

        #region Methods

        [Command("setting"), Alias("s")]
        public async Task GetSetting(string setting)
        {
        }

        [Command("setting"), Alias("s")]
        public async Task SaveSettings(string setting, string value)
        {
        }

        #endregion
    }
}
