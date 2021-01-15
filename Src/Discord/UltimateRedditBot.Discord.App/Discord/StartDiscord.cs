using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Services;
using UltimateRedditBot.Discord.Domain.Dtos;

namespace UltimateRedditBot.Discord.App.Discord
{
    public class StartDiscord
    {
        #region Fields

        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfiguration _config;
        private readonly IGuildService _guildService;

        #endregion

        #region Constructor

        public StartDiscord(IConfiguration config, CommandService commands, DiscordSocketClient discord, IServiceProvider provider, IGuildService guildService)
        {
            _config = config;
            _commands = commands;
            _discord = discord;
            _provider = provider;
            _guildService = guildService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Start the discord bot.
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            //Connect to discord.
            await Connect();

            //Load the modules
            await _commands.AddModulesAsync(typeof(UltimateCommandModule).Assembly, _provider);

            //Wait 1second to ensure the bot has connected to discord
            await Task.Delay(1000);

            //Set the bot's status.
            await _discord.SetGameAsync($"{ _discord.Guilds.Count }, servers", type: ActivityType.Watching);

            var guild = new GuildDto()
            {
                Id = _discord.Guilds.FirstOrDefault().Id
            };

            await _guildService.AddGuild(guild);
        }

        /// <summary>
        /// Connects the bot to discord.
        /// </summary>
        private async Task Connect()
        {
            // Get the discord token from the config file
            var discordToken = _config["AppKey"];

            //Ensure the bot is not empty.
            if (string.IsNullOrWhiteSpace(discordToken))
                throw new Exception("Please enter your bot's token into the `appsettings.json` file found in the applications root directory.");

            await _discord.LoginAsync(TokenType.Bot, discordToken);
            await _discord.StartAsync();
        }

        #endregion
    }
}
