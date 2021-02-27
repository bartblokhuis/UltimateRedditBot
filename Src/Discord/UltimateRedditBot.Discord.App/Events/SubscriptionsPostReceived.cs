using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.App.Services.Subscriptions;
using UltimateRedditBot.Discord.App.Services;
using UltimateRedditBot.Discord.Domain.Models;

namespace UltimateRedditBot.Discord.App.Events
{
    public class SubscriptionsPostReceived : IConsumer<IEnumerable<SubscriptionPost>>
    {
        #region Fields

        private readonly ITextChannelSubscriptionService _channelSubscriptionService;
        private readonly DiscordShardedClient _discord;

        #endregion

        #region Constructor

        public SubscriptionsPostReceived(ITextChannelSubscriptionService channelSubscriptionService, DiscordShardedClient discord)
        {
            _channelSubscriptionService = channelSubscriptionService;
            _discord = discord;
        }

        #endregion

        #region Methods

        public async Task HandleEvent(IEnumerable<SubscriptionPost> eventMessage)
        {
            var allSubscriptions = await _channelSubscriptionService.GetTextChannelSubscriptions();
            var allSubscriptionIds = allSubscriptions.Select(x => x.SubscriptionId).ToList();

            var subscriptionPosts = eventMessage.Where(x => allSubscriptionIds.Contains(x.Subscription.Id)).ToList();

            var sendMessageTasks = new List<Task>();
            foreach (var subscriptionPost in subscriptionPosts)
            {
                sendMessageTasks.AddRange(await SendDmPosts(subscriptionPost, allSubscriptions));
                sendMessageTasks.AddRange(SendGuildPosts(subscriptionPost, allSubscriptions));
            }

            await Task.WhenAll(sendMessageTasks);
        }

        #endregion

        #region Utils

        private IEnumerable<Task> SendGuildPosts(SubscriptionPost post, IEnumerable<TextChannelSubscription> textChannelSubscriptions)
        {
            textChannelSubscriptions = textChannelSubscriptions.Where(x => x.TextChannel.GuildId.HasValue).ToList();

            var tasks = new List<Task>();
            foreach (var textChannelSubscription in textChannelSubscriptions.Where(x => x.SubscriptionId == post.Subscription.Id))
            {
                var guild = _discord.GetGuild((ulong)textChannelSubscription.TextChannel.GuildId);

                var textChannel =
                    guild?.GetTextChannel(textChannelSubscription.TextChannel.TextChannelId);

                if (textChannel == null)
                    continue;

                tasks.Add((textChannel as ITextChannel)?.SendMessageAsync(post.PostDto.Url.ToString()));
            }

            return tasks;
        }

        private async Task<IEnumerable<Task>> SendDmPosts(SubscriptionPost post, IEnumerable<TextChannelSubscription> textChannelSubscriptions)
        {
            textChannelSubscriptions = textChannelSubscriptions.ToList();
            var userTextChannels = textChannelSubscriptions.Where(x => x.SubscriptionId == post.Subscription.Id && x.TextChannel.UserId.HasValue).Select(x => x.TextChannel).ToList();

            var tasks = new List<Task>();
            foreach (var userTextChannel in userTextChannels)
            {
                if (userTextChannel.UserId == null)
                    continue;

                var user = _discord.GetUser((ulong)userTextChannel.UserId);
                if(user != null)
                    tasks.Add(user.SendMessageAsync(post.PostDto.Url.ToString()));
            }

            return tasks;
        }

        #endregion

    }
}
