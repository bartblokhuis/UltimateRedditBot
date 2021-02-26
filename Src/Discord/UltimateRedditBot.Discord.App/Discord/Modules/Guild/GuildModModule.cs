using System;
using System.Linq;
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
        private readonly DiscordShardedClient _discord;

        #endregion

        #region Constructor

        public GuildModModule(IGuildModService guildModService, DiscordShardedClient discord)
        {
            _guildModService = guildModService;
            _discord = discord;
        }

        #endregion

        #region Methods

        [Command("Mod")]
        [Alias("Mod")]
        public async Task Mod(string userTag)
        {
            if (!userTag.StartsWith("<") || !userTag.EndsWith(">"))
            {
                await ReplyAsync("Invalid user.");
                return;
            }

            var userId = Convert.ToUInt64(userTag.Substring(3, userTag.Length - 4));

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
        public async Task Unmod(string userTag)
        {
            if (!userTag.StartsWith("<@!") || !userTag.EndsWith(">"))
            {
                await ReplyAsync("Invalid user.");
                return;
            }

            var userId = Convert.ToUInt64(userTag.Substring(3, userTag.Length - 4));

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
