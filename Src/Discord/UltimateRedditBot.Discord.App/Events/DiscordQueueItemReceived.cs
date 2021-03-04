using System;
using System.Diagnostics;
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
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Discord.App.Events
{
    public class DiscordQueueItemReceived : IConsumer<QueueItemPostReceived>
    {
        #region Fields

        private readonly DiscordShardedClient _discord;
        private readonly IPostHistoryService _postHistoryService;
        private readonly IPostService _postService;

        #endregion

        #region Constructor

        public DiscordQueueItemReceived(DiscordShardedClient discordShardedClient, IPostHistoryService postHistoryService, IPostService postService)
        {
            _discord = discordShardedClient;
            _postHistoryService = postHistoryService;
            _postService = postService;
        }

        #endregion

        #region Methods

        public async Task HandleEvent(QueueItemPostReceived eventMessage)
        {
            if (!(eventMessage.QueueClient is IDiscordQueueClient discordQueueClient))
                throw new ApplicationException("");

            eventMessage.PostDto.SubRedditId = eventMessage.QueueItem.SubredditDto.Id;

            var isForGuild = discordQueueClient.Group.Equals(DiscordSettings.GenericSettingGuildGroup);
            if (!isForGuild)
                await HandleDiscordDmEvent(discordQueueClient, eventMessage.PostDto);
            else
                await HandleDiscordGuildEvent(discordQueueClient, eventMessage.PostDto);

            //Since we still have to remove 1 in the queue service it means that here 1 is the last post item.
            if (eventMessage.QueueItem.AmountOfPosts == 1)
            {
                var postDto = await _postService.GetPostDtoById(eventMessage.PostDto.Id);

                if(postDto == null)
                    await _postService.SavePost(eventMessage.PostDto);
                
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
