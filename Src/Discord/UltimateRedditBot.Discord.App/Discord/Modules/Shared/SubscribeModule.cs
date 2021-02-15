using System.Threading.Tasks;
using Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Services.TextChannelService;
using UltimateRedditBot.Discord.Domain.Models;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Shared
{
    public class SubscribeModule : UltimateCommandModule
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
            TextChannel textChannel;
            if (Context.Guild != null)
            {
                textChannel = await _textChannelService.GetTextChannelById(Context.Channel.Id);
            }

            await ReplyAsync("patat");
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
