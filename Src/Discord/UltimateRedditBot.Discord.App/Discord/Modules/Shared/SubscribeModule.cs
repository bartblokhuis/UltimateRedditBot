using System;
using System.Threading.Tasks;
using Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Services.TextChannelService;
using UltimateRedditBot.Discord.Domain.Models;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Shared
{
    public class SubscribeModule : SharedCommandModule
    {
        #region Fields

        private readonly ITextChannelService _textChannelService;

        #endregion

        #region Constructor

        public SubscribeModule(ITextChannelService textChannelService)
        {
            _textChannelService = textChannelService;
        }

        #endregion

        #region Methods

        [Command("Subscribe")]
        [Alias("Sub")]
        public async Task Subscribe(string subreddit)
        {
            ulong? userId = IsForGuild() ? null : Context.User.Id;
            var textChannel = await _textChannelService.GetTextChannelById(Context.Channel.Id, Context.Guild?.Id, userId);

            if (textChannel == null)
            {
                await _textChannelService.RegisterTextChannel(Context.Channel.Id, Context.Guild?.Id, userId);
                textChannel = await _textChannelService.GetTextChannelById(Context.Channel.Id, Context?.Guild.Id, userId);
            }
        }

        [Command("unsubscribe")]
        [Alias("unsub")]
        public async Task Unsubscribe(string subreddit)
        {
            await ReplyAsync("patat 2");
        }

        #endregion
    }
}
