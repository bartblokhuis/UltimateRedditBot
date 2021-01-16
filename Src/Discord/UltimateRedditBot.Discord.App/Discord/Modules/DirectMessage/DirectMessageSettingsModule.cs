using System;
using System.Threading.Tasks;
using Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Services.User;
using UltimateRedditBot.Discord.Domain.Dtos;

namespace UltimateRedditBot.Discord.App.Discord.Modules.DirectMessage
{
    public class DirectMessageSettingsModule : UltimateDirectMessageModule
    {
        #region Fields

        private readonly IUserService _userService;

        #endregion

        #region Constructor

        public DirectMessageSettingsModule(IUserService userService)
        {
            _userService = userService;
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
                    //TODO Replace to message to default prefix
                    await ReplyAsync("User settings not found.");
                    return;
                }

                await ReplyAsync(userSettings.Prefix);
            }
        }

        [Command("setting"), Alias("s")]
        public async Task SaveSettings(string setting, string value)
        {
            if (setting.Equals("Prefix", StringComparison.OrdinalIgnoreCase))
            {
                //TODO Validate if the new prefix is valid
                var userSettings = await _userService.GetUserSettingsById(Context.User.Id);
                if (userSettings == null)
                {
                    userSettings = new UserSettingsDto(Context.User.Id)
                    {
                        Prefix = value
                    };
                }
                else
                    userSettings.Prefix = value;

                await _userService.SaveUserSettings(userSettings);
                await ReplyAsync($"Prefix is now: {value}");
            }
        }

        #endregion
    }
}
