using UltimateRedditBot.Discord.Domain.Dtos;

namespace UltimateRedditBot.Discord.App.Discord.Constants
{
    public static class DiscordSettings
    {
        public const string GenericSettingDmGroup = "DISCORD.DM";
        public const string GenericSettingGuildGroup = "DISCORD.GUILD";

        public static readonly GuildSettingsDto DefaultGuildSettings = new()
        {
            Prefix = "$"
        };
    }
}