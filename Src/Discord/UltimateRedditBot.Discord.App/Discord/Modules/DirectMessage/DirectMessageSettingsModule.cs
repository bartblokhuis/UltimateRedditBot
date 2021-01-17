using System;
using System.Threading.Tasks;
using Discord.Commands;
using UltimateRedditBot.Core.Constants;
using UltimateRedditBot.Discord.App.Discord.Constants;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Services.User;
using UltimateRedditBot.Discord.Domain.Dtos;
using UltimateRedditBot.Domain.Models.Settings;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Discord.App.Discord.Modules.DirectMessage
{
    public class DirectMessageSettingsModule : UltimateDirectMessageModule
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IGenericSettingService _genericSettingService;

        #endregion

        #region Constructor

        public DirectMessageSettingsModule(IUserService userService, IGenericSettingService genericSettingService)
        {
            _userService = userService;
            _genericSettingService = genericSettingService;
        }

        #endregion

        #region Methods

        [Command("setting"), Alias("s")]
        public async Task GetSetting(string setting)
        {
            if (setting.Equals("Prefix", StringComparison.OrdinalIgnoreCase))
            {
                var userSettings = await _userService.GetUserSettingsById(Context.User.Id);
                if (userSettings == null)
                {
                    await ReplyAsync($"Prefix: { DiscordSettings.DefaultGuildSettings.Prefix }");
                    return;
                }

                await ReplyAsync(userSettings.Prefix);
            }
            else if (setting.Equals("Bulk", StringComparison.OrdinalIgnoreCase))
            {
                var bulkLimit = await _genericSettingService.GetSettingValueByKeyGroupAndKey<int>(DiscordSettings.GenericSettingDmGroup, GenericSettingKeyConstants.BulkSettingKey, Context.User.Id.ToString());
                await ReplyAsync(bulkLimit.ToString());
            }
        }

        [Command("setting"), Alias("s")]
        public async Task SaveSettings(string setting, string value)
        {
            if (setting.Equals("Prefix", StringComparison.OrdinalIgnoreCase))
            {
                await UpdatePrefix(value);
                await ReplyAsync($"Prefix is now: {value}");
            }
            else if (setting.Equals("Bulk", StringComparison.OrdinalIgnoreCase))
            {
                await UpdateBulkSettings(value);
                await ReplyAsync($"Your bulk limit is now: {value}");
            }
        }

        #endregion

        #region Utils

        private async Task UpdatePrefix(string value)
        {
            //TODO Validate the prefix
            var userSettings = await _userService.GetUserSettingsById(Context.User.Id)
                               ?? new UserSettingsDto(Context.User.Id);

            userSettings.Prefix = value;

            await _userService.SaveUserSettings(userSettings);
        }

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

            var userId = Context.User.Id;
            var bulkSetting =
                await _genericSettingService.GetSettingByKeyGroupAndKey(DiscordSettings.GenericSettingDmGroup,
                    GenericSettingKeyConstants.BulkSettingKey, userId.ToString()) ?? new GenericSetting()
                {
                    KeyGroup = DiscordSettings.GenericSettingDmGroup,
                    Key = GenericSettingKeyConstants.BulkSettingKey,
                    EntityId = userId.ToString(),
                };

            bulkSetting.Value = value;

            await _genericSettingService.SaveSetting(bulkSetting);
        }

        #endregion
    }
}
