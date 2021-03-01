using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Discord.Modules.Helpers;
using UltimateRedditBot.Discord.App.Extensions.Microsoft;
using UltimateRedditBot.Discord.App.Services;
using UltimateRedditBot.Discord.App.Services.TextChannelService;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Domain.Enums;
using UltimateRedditBot.Domain.Models.Reddit;
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
        private readonly IBannedSubredditService _bannedSubredditService;
        private readonly DiscordShardedClient _discord;

        #endregion

        #region Constructor

        public SubscribeModule(ITextChannelService textChannelService, IRedditSubscriptionService redditSubscriptionService, ISubredditService subredditService, ITextChannelSubscriptionService channelSubscriptionService, DiscordShardedClient discord, IBannedSubredditService bannedSubredditService)
        {
            _textChannelService = textChannelService;
            _redditSubscriptionService = redditSubscriptionService;
            _subredditService = subredditService;
            _channelSubscriptionService = channelSubscriptionService;
            _discord = discord;
            _bannedSubredditService = bannedSubredditService;
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

            if (Context.Guild != null && _bannedSubredditService.IsSubredditBanned(Context.Guild.Id, subreddit))
            {
                await ReplyAsync("This subreddit is banned");
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
                await ReplyAsync(lastPostUrl);


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
            var textChannelSubscriptions =  await _channelSubscriptionService.GetTextChannelSubscriptions();

            List<TextChannelSubscription> subscriptions;

            var isForGuild = IsForGuild();

            subscriptions = isForGuild ?
                textChannelSubscriptions.Where(x => x.TextChannel.GuildId == Context.Guild.Id).ToList()
                : textChannelSubscriptions.Where(x => x.TextChannel.UserId == Context.User.Id).ToList();

            if (!subscriptions.Any())
            {
                await ReplyAsync("There are no subscriptions");
                return;
            }

            var embedBuilder = new EmbedBuilder
            {
                Title = isForGuild ? $"Subscriptions in {Context.Guild.Name}:" : $"Subscriptions for {Context.User.Username}",
                Fields = isForGuild? await GuildSubscriptionFields(subscriptions) : await DmSubscriptionFields(textChannelSubscriptions)
            };

            await ReplyAsync("", false, embedBuilder.Build());
        }



        #endregion


        #region Utils

        private async Task<List<EmbedFieldBuilder>> DmSubscriptionFields(List<TextChannelSubscription> textChannelSubscriptions)
        {
            var fields = new List<EmbedFieldBuilder>();

            var subscriptions =
                await _redditSubscriptionService.GetSubscriptionByIds(textChannelSubscriptions.Select(x => x.SubscriptionId));

            var valueBuilder = new StringBuilder();

            foreach (var subscription in subscriptions)
                valueBuilder.Append($"{subscription?.Subreddit?.Name}, {subscription?.Sort} \n");

            if (valueBuilder.Length == 0)
                return fields;

            fields.Add(new EmbedFieldBuilder
            {
                Name = "Subs",
                Value = valueBuilder.ToString()
            });

            return fields;
        }

        private async Task<List<EmbedFieldBuilder>> GuildSubscriptionFields(List<TextChannelSubscription> textChannelSubscriptions)
        {
            var channels = textChannelSubscriptions.DistinctBy(x => x.TextChannel?.TextChannelId)
                .Select(x => new
                {
                    ChannelId = x.TextChannel.TextChannelId,
                    Subscriptions = textChannelSubscriptions
                        .Where(y => y.TextChannel.TextChannelId == x.TextChannel.TextChannelId)
                        .Select(y => y.SubscriptionId)
                }).ToList();

            var subscriptions =
                await _redditSubscriptionService.GetSubscriptionByIds(channels.SelectMany(x => x.Subscriptions));

            var fields = new List<EmbedFieldBuilder>();

            subscriptions = subscriptions.ToList();
            foreach (var channel in channels)
            {
                var t = IsForGuild()
                    ? await Context.Guild.GetChannelAsync(channel.ChannelId) as ITextChannel
                    : await _discord.GetDMChannelAsync(channel.ChannelId) as ITextChannel;

                var discordChannel = await Context.Guild.GetChannelAsync(channel.ChannelId);
                if (discordChannel == null)
                    continue;

                var subsInChannel = subscriptions.Where(x => channel.Subscriptions.Contains(x.Id)).ToList();

                var valueBuilder = new StringBuilder();
                foreach (var sub in subsInChannel)
                    valueBuilder.Append($"{sub?.Subreddit?.Name}, {sub?.Sort} \n");

                if (valueBuilder.Length == 0)
                    continue;

                fields.Add(new EmbedFieldBuilder
                {
                    Name = discordChannel.Name,
                    Value = valueBuilder.ToString()
                });
            }

            return fields;
        }

        #endregion
    }
}
