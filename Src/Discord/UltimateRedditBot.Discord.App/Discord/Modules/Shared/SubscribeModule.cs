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
        private readonly IPostService _postService;

        #endregion

        #region Constructor

        public SubscribeModule(ITextChannelService textChannelService, IRedditSubscriptionService redditSubscriptionService, ISubredditService subredditService, ITextChannelSubscriptionService channelSubscriptionService, IPostService postService)
        {
            _textChannelService = textChannelService;
            _redditSubscriptionService = redditSubscriptionService;
            _subredditService = subredditService;
            _channelSubscriptionService = channelSubscriptionService;
            _postService = postService;
        }

        #endregion

        #region Methods

        #region Subscriptions

        [Command("Subscribe")]
        [Alias("Sub")]
        public async Task Subscribe(string subredditName)
        {
            await Subscribe(subredditName, Sort.Hot);
        }

        [Command("Subscribe")]
        [Alias("Sub")]
        public async Task Subscribe(string subredditName, string sort)
        {
            if(Enum.TryParse(typeof(Sort), sort, true, out var enumSort))
                await Subscribe(subredditName, (Sort)enumSort);
        }

        private async Task Subscribe(string subredditName, Sort sort)
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

            if (Context.Guild != null && subreddit.IsNsfw && !(Context.Channel is ITextChannel && ((ITextChannel) Context.Channel).IsNsfw))
            {
                await ReplyAsync("Can't subscribe to an nsfw channel in a non nsfw chat.");
                return;
            }

            var subscription = await _redditSubscriptionService.GetSubscriptionBySubredditAndSort(subreddit.Id, sort);
            subscription ??= await _redditSubscriptionService.CreateAndGetSubscription(subreddit.Id, sort);


            if (await _channelSubscriptionService.IsSubscribed(textChannel.Id, subscription.Id))
            {
                await ReplyAsync("Already subscribed");
                return;
            }

            await _channelSubscriptionService.Subscribe(textChannel.Id, subscription.Id);

            var lastPostUrl = subscription?.Post?.Url.ToString();
            if (!string.IsNullOrEmpty(lastPostUrl))
            {
                await ReplyAsync(lastPostUrl);
            }

        }

        #endregion

        #region Unsub

        [Command("unsubscribe")]
        [Alias("unsub")]
        public async Task Unsubscribe(string subreddit)
        {
            await Unsubscribe(subreddit, Sort.Hot);
        }

        [Command("unsubscribe")]
        [Alias("unsub")]
        public async Task Unsubscribe(string subreddit, string sort)
        {
            if(Enum.TryParse(typeof(Sort), sort, true, out var enumSort))
                await Unsubscribe(subreddit, (Sort)enumSort);
        }

        private async Task Unsubscribe(string subredditName, Sort sort)
        {
            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            if (subreddit == null)
            {
                await ReplyAsync("Subreddit not found");
                return;
            }

            ulong? userId = IsForGuild() ? null : Context.User.Id;
            var textChannel = await _textChannelService.GetTextChannelById(Context.Channel.Id, Context.Guild?.Id, userId);
            var subscription = await _redditSubscriptionService.GetSubscriptionBySubredditAndSort(subreddit.Id, sort);
            if (subscription == null || textChannel == null)
            {
                await ReplyAsync($"Not subscribed to {subredditName}");
                return;
            }

            var textChannelSubscription =
                await _channelSubscriptionService.GetTextChannelSubscription(textChannel.Id, subscription.Id);

            if (textChannelSubscription == null)
            {
                await ReplyAsync($"Not subscribed to {subredditName}");
                return;
            }

            await _channelSubscriptionService.Unsubscribe(textChannelSubscription);
            await ReplyAsync("Unsubscribed");
        }

        #endregion


        [Command("subscriptions")]
        [Alias("subs")]
        public async Task Subscriptions()
        {
            await ReplyAsync("");
        }



        #endregion
    }
}
