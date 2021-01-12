using Discord.Commands;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Common
{
    //For now we only allow guild's to interact with the bot.
    [RequireContext(ContextType.Guild)]
    public class UltimateCommandModule : ModuleBase
    {
        //TODO Add logger
    }
}
