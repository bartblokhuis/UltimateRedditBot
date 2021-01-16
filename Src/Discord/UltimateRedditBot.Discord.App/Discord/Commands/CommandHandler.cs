using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Discord.Commands;
using Discord.WebSocket;
using UltimateRedditBot.Discord.App.Discord.Constants;
using UltimateRedditBot.Discord.App.Services;

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

        #endregion

        public CommandHandler(DiscordSocketClient discord,
            CommandService commands,
            IConfiguration config,
            IServiceProvider provider,
            IGuildService guildService)
        {
            _discord = discord;
            _commands = commands;
            _config = config;
            _provider = provider;
            _guildService = guildService;

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
            var result = await _commands.ExecuteAsync(context, argPos, _provider);

            //If sending the message was not successful send the error message.
            if (!result.IsSuccess)
            {
                var errorMessage = result.Error switch
                {
                    CommandError.UnknownCommand => "Unknown command",
                    CommandError.BadArgCount => "The command has to many or not enough arguments",
                    CommandError.Exception => "Bot error has occured, please try again. If the error keeps happening please contact support.",
                    _ => "Failed to execute command"
                };

                await context.Channel.SendMessageAsync(errorMessage);
            }
        }

        private async Task<string> GetPrefix(ulong id, bool isGuild)
        {
            var prefix = DefaultSettings.DefaultGuildSettings.Prefix;
            if (isGuild)
            {
                var guildSettings = await _guildService.GetGuildSettingsById(id);
                if (guildSettings is not null && !string.IsNullOrEmpty(guildSettings.Prefix))
                    prefix = guildSettings.Prefix;
            }

            return prefix;
        }

        #endregion
    }
}
