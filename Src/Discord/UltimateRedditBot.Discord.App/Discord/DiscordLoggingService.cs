using System;
using System.Linq;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace UltimateRedditBot.Discord.App.Discord
{
    public class DiscordLoggingService
    {
	    private readonly ILogger<DiscordLoggingService> _logger;

		public DiscordLoggingService(DiscordShardedClient client, CommandService command, ILogger<DiscordLoggingService> logger)
		{
			_logger = logger;
			client.Log += LogAsync;
			command.Log += LogAsync;
		}
		private Task LogAsync(LogMessage message)
		{
			if (message.Exception is CommandException cmdException)
			{
				_logger.LogError($"[Command/{message.Severity}] {cmdException.Command.Aliases.First()}"
				                   + $" failed to execute in {cmdException.Context.Channel}.");
				_logger.LogError(cmdException.Message);
			}
			else
				_logger.LogInformation($"[General/{message.Severity}] {message}");

			return Task.CompletedTask;
		}
	}
}
