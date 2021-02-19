using System;
using System.Threading.Tasks;
using Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Services.TextChannelService;
using UltimateRedditBot.Domain.Enums;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Shared
{
    public class SubscribeModule : SharedCommandModule
    {
        #region Fields

        private readonly ITextChannelService _textChannelService;
        private readonly ISubredditService _subredditService;
        private readonly IRedditSubscriptionService _redditSubscriptionService;

        #endregion

        #region Constructor

        public SubscribeModule(ITextChannelService textChannelService, IRedditSubscriptionService redditSubscriptionService, ISubredditService subredditService)
        {
            _textChannelService = textChannelService;
            _redditSubscriptionService = redditSubscriptionService;
            _subredditService = subredditService;
        }

        #endregion

        #region Methods

        [Command("Subscribe")]
        [Alias("Sub")]
        public async Task Subscribe(string subredditName)
        {
            ulong? userId = IsForGuild() ? null : Context.User.Id;
            var textChannel = await _textChannelService.GetTextChannelById(Context.Channel.Id, Context.Guild?.Id, userId);

            if (textChannel == null)
            {
                await _textChannelService.RegisterTextChannel(Context.Channel.Id, Context.Guild?.Id, userId);
                textChannel = await _textChannelService.GetTextChannelById(Context.Channel.Id, Context?.Guild.Id, userId);
            }

            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            if (subreddit == null)
            {
                await ReplyAsync("Subreddit doesn't exist");
                return;
            }

            //TODO Make sort configurable by settings and overwrite method
            var subscriptions = await _redditSubscriptionService.GetSubscriptionBySubredditAndSort(subreddit.Id, Sort.Hot);
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
