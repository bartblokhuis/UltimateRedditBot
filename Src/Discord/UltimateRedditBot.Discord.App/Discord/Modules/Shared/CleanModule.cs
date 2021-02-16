using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Shared
{
    public class CleanModule : SharedCommandModule

    {

    #region Fields

    private readonly DiscordSocketClient _discord;

    #endregion

    #region constructor

    public CleanModule(DiscordSocketClient discord)
    {
        _discord = discord;
    }

    #endregion

    #region Commands

    [Command("clean")]
    public async Task Clean(int amountOfMessagesToScan)
    {
        if (amountOfMessagesToScan > 100)
            amountOfMessagesToScan = 100;

        var messages = await Context.Channel.GetMessagesAsync(limit: amountOfMessagesToScan).ToListAsync();
        var removeTasks = new List<Task>();
        foreach (var message in messages.SelectMany(x => x))
        {
            if (message.Author.Id != _discord.CurrentUser.Id || message.IsPinned)
                continue;

            removeTasks.Add(Context.Channel.DeleteMessageAsync(message));
        }

        await Task.WhenAll(removeTasks);
        await ReplyAsync($"Removed {removeTasks.Count} messages");
    }

    #endregion

    }
}
