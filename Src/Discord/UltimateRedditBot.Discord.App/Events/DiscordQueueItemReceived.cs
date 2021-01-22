using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.App.Services.Queue;
using UltimateRedditBot.Discord.App.Discord.Constants;
using UltimateRedditBot.Discord.App.Services.Queue;
using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Domain.Queue;

namespace UltimateRedditBot.Discord.App.Events
{
    public class DiscordQueueItemReceived : IConsumer<QueueItemPostReceived>
    {
        #region Fields

        private readonly DiscordSocketClient _discordSocketClient;

        #endregion

        #region Constructor

        public DiscordQueueItemReceived(DiscordSocketClient discordSocketClient)
        {
            _discordSocketClient = discordSocketClient;
        }

        #endregion

        #region Methods

        public async Task HandleEvent(QueueItemPostReceived eventMessage)
        {
            if (!(eventMessage.QueueClient is IDiscordQueueClient discordQueueClient))
                throw new ApplicationException("");

            if (discordQueueClient.Group.Equals(DiscordSettings.GenericSettingDmGroup))
                await HandleDiscordDmEvent(discordQueueClient, eventMessage.PostDto);
            else
                await HandleDiscordGuildEvent(discordQueueClient, eventMessage.PostDto);
        }

        #endregion

        #region Utils

        private async Task HandleDiscordDmEvent(IQueueClient queueClient, PostDto postDto)
        {
            var dmChannel = _discordSocketClient.DMChannels.FirstOrDefault(x =>
                x.Users.FirstOrDefault(y => y.Id == queueClient.ClientId) != null);

            if (dmChannel != null)
                await dmChannel.SendMessageAsync(postDto.Url.ToString());
        }

        private async Task HandleDiscordGuildEvent(IDiscordQueueClient queueClient, PostDto postDto)
        {
            var guild = _discordSocketClient.Guilds.FirstOrDefault(x => x.Id == queueClient.ClientId);

            if (!(guild?.Channels.FirstOrDefault(x => x.Id == queueClient.ChannelId) is ITextChannel channel))
                return;

            channel.EnterTypingState();
            await channel.SendMessageAsync(postDto.Url.ToString());
        }

        #endregion


    }
}
