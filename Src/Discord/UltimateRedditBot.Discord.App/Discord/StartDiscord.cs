using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Services.Guild;
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
        private readonly ILogger<StartDiscord> _logger;

        #endregion

        #region Constructor

        public StartDiscord(IConfiguration config, CommandService commands, DiscordSocketClient discord,
            IServiceProvider provider, IGuildService guildService, ILogger<StartDiscord> logger)
        {
            _config = config;
            _commands = commands;
            _discord = discord;
            _provider = provider;
            _guildService = guildService;
            _logger = logger;

            _discord.JoinedGuild += OnGuildJoin;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Start the discord bot.
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            //Connect to discord.
            await Connect();

            //Load the modules
            await _commands.AddModulesAsync(typeof(UltimateCommandModule).Assembly, _provider);

            //Wait 1second to ensure the bot has connected to discord
            await Task.Delay(2000);

            //Set the bot's status.
            await _discord.SetGameAsync($"{_discord.Guilds.Count}, servers", type: ActivityType.Watching);

            await RegisterNewGuilds();
        }

        /// <summary>
        ///     Connects the bot to discord.
        /// </summary>
        private async Task Connect()
        {
            // Get the discord token from the config file
            var discordToken = _config["AppKey"];

            //Ensure the bot is not empty.
            if (string.IsNullOrWhiteSpace(discordToken))
                throw new Exception(
                    "Please enter your bot's token into the `appsettings.json` file found in the applications root directory.");

            _logger.LogInformation("Connecting to discord...");
            await _discord.LoginAsync(TokenType.Bot, discordToken);
            await _discord.StartAsync();
            _logger.LogInformation("Connected to discord.");
        }

        private async Task RegisterNewGuilds()
        {
            if (!_discord.Guilds.Any())
                return;

            var newGuildDtos = _discord.Guilds.Select(guild => new GuildDto
            {
                Id = guild.Id
            });

            await _guildService.RegisterNewGuilds(newGuildDtos);
        }

        /// <summary>
        ///     Handles the on guild joined event.
        ///     Every guild that joins has to ge registered in the database.
        /// </summary>
        /// <param name="socketGuild">socketGuild event</param>
        /// <returns></returns>
        private async Task OnGuildJoin(SocketGuild socketGuild)
        {
            var guild = await _guildService.GetById(socketGuild.Id);
            if (guild == null)
                await _guildService.InsertGuild(new GuildDto {Id = socketGuild.Id});
        }

        #endregion
    }
}
