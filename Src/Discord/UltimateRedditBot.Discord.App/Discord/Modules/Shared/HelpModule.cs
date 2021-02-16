using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Shared
{
    public class HelpModule : SharedCommandModule
    {
        #region Fields

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor

        public HelpModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Methods

        [Command("help")]
        [Alias("help")]
        public async Task Help()
        {
        }

        [Command("invite")]
        [Alias("inv")]
        public async Task Invite()
        {
            var inviteLink = _configuration["InviteLink"];

            if (string.IsNullOrEmpty(inviteLink))
            {
                await ReplyAsync("No invite link has been set.");
                return;
            }

            await ReplyAsync(inviteLink);
        }

        #endregion
    }
}
