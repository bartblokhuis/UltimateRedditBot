using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Discord.Commands;
using Discord.WebSocket;
using UltimateRedditBot.Discord.App.Discord.Constants;
using UltimateRedditBot.Discord.App.Services;
using UltimateRedditBot.Discord.App.Services.Guild;
using UltimateRedditBot.Discord.App.Services.User;

namespace UltimateRedditBot.Discord.App.Discord.Commands
{
    public class CommandHandler
    {
        #region Fields

        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _provider;
        private readonly IGuildService _guildService;
        private readonly IUserService _userService;

        #endregion

        public CommandHandler(DiscordSocketClient discord,
            CommandService commands,
            IConfiguration config,
            IServiceProvider provider,
            IGuildService guildService, IUserService userService)
        {
            _discord = discord;
            _commands = commands;
            _config = config;
            _provider = provider;
            _guildService = guildService;
            _userService = userService;

            _discord.MessageReceived += OnMessageReceivedAsync;
        }

        #region Methods

        /// <summary>
        /// Used to handle the on message received call.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            //Ensure that the message is sent by a user.
            if (!(s is SocketUserMessage msg) || msg.Author.Id == _discord.CurrentUser.Id)
                return;

            //Set the command context.
            var context = new SocketCommandContext(_discord, msg);

            //Ensure that the message being sent had the configured prefix.
            var argPos = 0;

            string prefix;
            if (context.Guild != null)
                prefix = await GetPrefix(context.Guild.Id, true);
            else
                prefix = await GetPrefix(context.User.Id, false);

            if (!msg.HasStringPrefix(prefix, ref argPos) && !msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
                return;

            //Execute the command
            try
            {
                var result = await _commands.ExecuteAsync(context, argPos, _provider);
                //If sending the message was not successful send the error message.
                if (!result.IsSuccess)
                {
                    var errorMessage = result.Error switch
                    {
                        CommandError.UnknownCommand => "Unknown command",
                        CommandError.BadArgCount => "The command has to many or not enough arguments",
                        CommandError.Exception =>
                            "Bot error has occured, please try again. If the error keeps happening please contact support.",
                        _ => "Failed to execute command"
                    };

                    await context.Channel.SendMessageAsync(errorMessage);
                }
            }
            catch (Exception e)
            {

            }



        }

        private async Task<string> GetPrefix(ulong id, bool isGuild)
        {
            var prefix = DiscordSettings.DefaultGuildSettings.Prefix;
            if (isGuild)
            {
                var guildSettings = await _guildService.GetGuildSettingsById(id);
                if (guildSettings is not null && !string.IsNullOrEmpty(guildSettings.Prefix))
                    prefix = guildSettings.Prefix;
            }
            else
            {
                var userSettings = await _userService.GetUserSettingsById(id);
                if (userSettings is not null && !string.IsNullOrEmpty(userSettings.Prefix))
                    prefix = userSettings.Prefix;
            }

            return prefix;
        }

        #endregion
    }
}
