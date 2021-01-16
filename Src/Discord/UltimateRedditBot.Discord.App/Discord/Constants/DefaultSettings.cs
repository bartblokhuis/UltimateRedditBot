using UltimateRedditBot.Discord.Domain.Dtos;

namespace UltimateRedditBot.Discord.App.Discord.Constants
{
    public static class DefaultSettings
    {
        public static readonly GuildSettingsDto DefaultGuildSettings = new GuildSettingsDto()
        {
            Prefix = "$"
        };
    }
}
