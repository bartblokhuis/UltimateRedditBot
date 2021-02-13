using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using UltimateRedditBot.Core.Constants;
using UltimateRedditBot.Discord.App.Discord.Constants;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Services;
using UltimateRedditBot.Discord.App.Services.Guild;
using UltimateRedditBot.Discord.Domain.Dtos;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Domain.Models.Settings;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Guild
{
    public class GuildSettingsModule : UltimateGuildModule
    {
        #region Constructor

        public GuildSettingsModule(IGuildService guildService, IGenericSettingService genericSettingService, IGuildModService guildModService)
        {
            _guildService = guildService;
            _genericSettingService = genericSettingService;
            _guildModService = guildModService;
        }

        #endregion

        #region Fields

        private readonly IGuildService _guildService;
        private readonly IGenericSettingService _genericSettingService;
        private readonly IGuildModService _guildModService;

        #endregion

        #region Utils

        private async Task UpdateBulkSettings(string value)
        {
            if (!int.TryParse(value, out var newBulkLimit))
            {
                await ReplyAsync("The bulk limit has to be a number");
                return;
            }

            if (newBulkLimit < 0)
            {
                await ReplyAsync("The minimum limit is 0");
                return;
            }

            var guildId = Context.Guild.Id;
            var bulkSetting =
                await _genericSettingService.GetSettingByKeyGroupAndKey(DiscordSettings.GenericSettingGuildGroup,
                    GenericSettingKeyConstants.BulkSettingKey, guildId.ToString()) ?? new GenericSetting
                {
                    KeyGroup = DiscordSettings.GenericSettingGuildGroup,
                    Key = GenericSettingKeyConstants.BulkSettingKey,
                    EntityId = guildId.ToString()
                };

            bulkSetting.Value = value;

            await _genericSettingService.SaveSetting(bulkSetting);
        }

        private bool IsMod()
        {
            return _guildModService.IsMod(Context.User.Id, Context.Guild.Id);
        }

        #endregion

        #region Methods

        [Command("setting")]
        [Alias("s")]
        public async Task GetSetting(string setting)
        {
            var guild = ((SocketGuildChannel) Context.Channel).Guild;
            if (setting.Equals("Prefix", StringComparison.OrdinalIgnoreCase))
            {
                var guildPrefix = _guildService.GetPrefix(guild.Id);
                if (string.IsNullOrEmpty(guildPrefix))
                {
                    await ReplyAsync($"Prefix: {DiscordSettings.DefaultGuildSettings.Prefix}");
                    return;
                }

                await ReplyAsync(guildPrefix);
            }
            else if (setting.Equals("Bulk", StringComparison.OrdinalIgnoreCase))
            {
                var bulkLimit = await _genericSettingService.GetSettingValueByKeyGroupAndKey<int>(
                    DiscordSettings.GenericSettingGuildGroup, GenericSettingKeyConstants.BulkSettingKey,
                    Context.Guild.Id.ToString());
                await ReplyAsync(bulkLimit.ToString());
            }
        }

        [Command("setting")]
        [Alias("s")]
        public async Task SaveSettings(string setting, string value)
        {
            if (!IsMod())
            {
                await ReplyAsync("Only mods can edit settings");
                return;
            }

            var guild = ((SocketGuildChannel) Context.Channel).Guild;
            if (setting.Equals("Prefix", StringComparison.OrdinalIgnoreCase))
            {
                //TODO Check if the user attempting to change the setting has the permissions to do so.
                var guildSettings = _guildService.GetGuildSettingsById(guild.Id);
                if (guildSettings == null)
                    guildSettings = new GuildSettings
                    {
                        GuildId = guild.Id,
                        Prefix = value
                    };
                else
                    guildSettings.Prefix = value;

                await _guildService.SaveGuildSettings(guildSettings);
                await ReplyAsync($"Prefix is now: {value}");
            }
            else if (setting.Equals("Bulk", StringComparison.OrdinalIgnoreCase))
            {
                await UpdateBulkSettings(value);
                await ReplyAsync($"Server bulk limit is now: {value}");
            }
        }

        #endregion
    }
}
