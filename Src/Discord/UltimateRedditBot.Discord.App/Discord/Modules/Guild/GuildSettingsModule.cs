using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Services;
using UltimateRedditBot.Discord.Domain.Dtos;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Guild
{
    public class GuildSettingsModule : UltimateGuildModule
    {
        #region Fields

        private readonly IGuildService _guildService;

        #endregion

        #region Constructor

        public GuildSettingsModule(IGuildService guildService)
        {
            _guildService = guildService;
        }

        #endregion

        #region Methods

        [Command("setting"), Alias("s")]
        public async Task GetSetting(string setting)
        {
            var guild = ((SocketGuildChannel)Context.Channel).Guild;
            if (setting.Equals("Prefix", StringComparison.OrdinalIgnoreCase))
            {
                var guildSettings = await _guildService.GetGuildSettingsById(guild.Id);
                if (guildSettings == null)
                {
                    //TODO Replace to message to default prefix
                    await ReplyAsync("Guild settings not found.");
                    return;
                }

                await ReplyAsync(guildSettings.Prefix);
            }


        }

        [Command("setting"), Alias("s")]
        public async Task SaveSettings(string setting, string value)
        {
            var guild = ((SocketGuildChannel)Context.Channel).Guild;
            if (setting.Equals("Prefix", StringComparison.OrdinalIgnoreCase))
            {
                var guildSettings = await _guildService.GetGuildSettingsById(guild.Id);
                if (guildSettings == null)
                {
                    guildSettings = new GuildSettingsDto(guild.Id)
                    {
                        Prefix = value
                    };
                }
                else
                    guildSettings.Prefix = value;

                await _guildService.SaveGuildSettings(guildSettings);

            }
        }

        #endregion
    }
}
