using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Services;
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
        private readonly ITextChannelSubscriptionService _channelSubscriptionService;

        #endregion

        #region Constructor

        public SubscribeModule(ITextChannelService textChannelService, IRedditSubscriptionService redditSubscriptionService, ISubredditService subredditService, ITextChannelSubscriptionService channelSubscriptionService)
        {
            _textChannelService = textChannelService;
            _redditSubscriptionService = redditSubscriptionService;
            _subredditService = subredditService;
            _channelSubscriptionService = channelSubscriptionService;
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
                textChannel = await _textChannelService.GetTextChannelById(Context.Channel.Id, Context?.Guild?.Id, userId);
            }

            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            if (subreddit == null)
            {
                await ReplyAsync("Subreddit doesn't exist");
                return;
            }

            if (subreddit.IsNsfw && Context.Channel is ITextChannel && ((ITextChannel) Context.Channel).IsNsfw)
            {
                await ReplyAsync("Can't subscribe to an nsfw channel in a non nsfw chat.");
                return;
            }

            //TODO Make sort configurable by settings and overwrite method
            var subscription = await _redditSubscriptionService.GetSubscriptionBySubredditAndSort(subreddit.Id, Sort.Hot);
            subscription ??= await _redditSubscriptionService.CreateAndGetSubscription(subreddit.Id, Sort.Hot);


            if (await _channelSubscriptionService.IsSubscribed(textChannel.Id, subscription.Id))
            {
                await ReplyAsync("Already subscribed");
                return;
            }

            await _channelSubscriptionService.Subscribe(textChannel.Id, subscription.Id);
            await ReplyAsync($"Subscribed to {subreddit.Name}");
        }

        [Command("unsubscribe")]
        [Alias("unsub")]
        public async Task Unsubscribe(string subreddit)
        {
            await ReplyAsync("");
        }

        #endregion
    }
}
