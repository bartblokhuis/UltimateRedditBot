using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Services;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Guild
{
    public class GuildModModule : UltimateGuildModule
    {
        #region Fields

        private readonly IGuildModService _guildModService;

        #endregion

        #region Constructor

        public GuildModModule(IGuildModService guildModService)
        {
            _guildModService = guildModService;
        }

        #endregion

        #region Methods

        [Command("Mod")]
        [Alias("Mod")]
        public async Task Mod(IUser user = null)
        {
            if (user == null)
            {
                await ReplyAsync("Invalid user.");
                return;
            }

            var userId = user.Id;

            if (!_guildModService.IsMod(Context.User.Id, Context.Guild.Id) && Context.User.Id != Context.Guild.OwnerId)
            {
                await ReplyAsync("Only mods or the server owner can manage bot permissions");
                return;
            }

            var result = await _guildModService.Mod(userId, Context.Guild.Id);
            await ReplyAsync((string.IsNullOrEmpty(result)) ? "Is now a mod" : result);
        }

        [Command("Unmod")]
        [Alias("Unmod")]
        public async Task Unmod(IUser user = null)
        {
            if (user == null)
            {
                await ReplyAsync("Invalid user.");
                return;
            }

            var userId = user.Id;
            if (Context.Guild.OwnerId == userId)
            {
                await ReplyAsync("Nice try!");
                return;
            }

            var result = await _guildModService.UnMod(userId, Context.Guild.Id);
            await ReplyAsync((string.IsNullOrEmpty(result)) ? $"Is no longer a mod" : result);
        }

        #endregion
    }
}
