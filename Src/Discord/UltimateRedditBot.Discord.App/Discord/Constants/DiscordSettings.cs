using UltimateRedditBot.Discord.Domain.Dtos;

namespace UltimateRedditBot.Discord.App.Discord.Constants
{
    public static class DiscordSettings
    {
        public static readonly GuildSettingsDto DefaultGuildSettings = new()
        {
            Prefix = "$"
        };

        public const string GenericSettingDmGroup = "DISCORD.DM";
        public const string GenericSettingGuildGroup = "DISCORD.GUILD";
    }
}
