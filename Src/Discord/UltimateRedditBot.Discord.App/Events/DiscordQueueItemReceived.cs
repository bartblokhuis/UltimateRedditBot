using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.App.Services.Queue;
using UltimateRedditBot.Discord.App.Discord.Constants;
using UltimateRedditBot.Discord.App.Services;
using UltimateRedditBot.Discord.App.Services.Queue;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.Discord.App.Events
{
    public class DiscordQueueItemReceived : IConsumer<QueueItemPostReceived>
    {
        #region Constructor

        public DiscordQueueItemReceived(DiscordShardedClient DiscordShardedClient, IPostHistoryService postHistoryService)
        {
            _discord = DiscordShardedClient;
            _postHistoryService = postHistoryService;
        }

        #endregion

        #region Methods

        public async Task HandleEvent(QueueItemPostReceived eventMessage)
        {
            if (!(eventMessage.QueueClient is IDiscordQueueClient discordQueueClient))
                throw new ApplicationException("");

            var isForGuild = discordQueueClient.Group.Equals(DiscordSettings.GenericSettingGuildGroup);
            if (!isForGuild)
                await HandleDiscordDmEvent(discordQueueClient, eventMessage.PostDto);
            else
                await HandleDiscordGuildEvent(discordQueueClient, eventMessage.PostDto);

            //Since we still have to remove 1 in the queue service it means that here 1 is the last post item.
            if (eventMessage.QueueItem.AmountOfPosts == 1)
            {
                var id = Convert.ToUInt64(eventMessage.QueueClient.ClientId);

                var postHistory = _postHistoryService.GetPostHistory(isForGuild, id,
                    eventMessage.QueueItem.SubredditDto.Id);

                if (postHistory != null)
                {
                    postHistory.PostId = eventMessage.PostDto.Id;
                    await _postHistoryService.SavePostHistory(postHistory);
                    return;
                }

                postHistory = new PostHistory
                {
                    PostId = eventMessage.PostDto.Id,
                    SubredditId = eventMessage.QueueItem.SubredditDto.Id
                };

                if (isForGuild)
                    postHistory.GuildId = id;
                else
                    postHistory.UserId = id;

                await _postHistoryService.SavePostHistory(postHistory);
            }
        }

        #endregion

        #region Fields

        private readonly DiscordShardedClient _discord;
        private readonly IPostHistoryService _postHistoryService;

        #endregion

        #region Utils

        private async Task HandleDiscordDmEvent(IQueueClient queueClient, PostDto postDto)
        {
            var user = _discord.GetUser(queueClient.ClientId);
            if (user != null)
                await user.SendMessageAsync(postDto.Url.ToString());
        }

        private async Task HandleDiscordGuildEvent(IDiscordQueueClient queueClient, PostDto postDto)
        {
            var guild = _discord.GetGuild(queueClient.ClientId);

            Debug.Assert(queueClient.ChannelId != null, "queueClient.ChannelId != null");
            var channel = guild?.GetTextChannel((ulong)queueClient.ChannelId);

            if (channel == null)
                return;

            await channel.TriggerTypingAsync();
            await channel.SendMessageAsync(postDto.Url.ToString());

        }

        #endregion
    }
}
