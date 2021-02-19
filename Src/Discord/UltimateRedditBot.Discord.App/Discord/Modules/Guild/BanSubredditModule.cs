using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using UltimateRedditBot.Discord.App.Discord.Modules.Common;
using UltimateRedditBot.Discord.App.Discord.Modules.Helpers;
using UltimateRedditBot.Discord.App.Services;
using UltimateRedditBot.Domain.Dtos.Reddit;

namespace UltimateRedditBot.Discord.App.Discord.Modules.Guild
{
    public class BanSubredditModule : UltimateGuildModule
    {
        #region Fields

        private readonly IBannedSubredditService _bannedSubredditService;
        private readonly IGuildModService _guildModService;

        #endregion

        #region Constructor

        public BanSubredditModule(IBannedSubredditService bannedSubredditService, IGuildModService guildModService)
        {
            _bannedSubredditService = bannedSubredditService;
            _guildModService = guildModService;
        }

        #endregion

        #region Commands

        [Command("ban")]
        [Alias("b")]
        public async Task BanSubreddit(string subreddit)
        {
            if (!IsMod())
            {
                await ReplyAsync("Only bot mods can manage subreddit bans");
                return;
            }

            var result = await _bannedSubredditService.BanSubreddit(Context.Guild.Id, subreddit);
            await ReplyAsync(result);
        }


        [Command("unban")]
        [Alias("ub")]
        public async Task UnbanSubreddit(string subreddit)
        {
            if (!IsMod())
            {
                await ReplyAsync("Only bot mods can manage subreddit bans");
                return;
            }

            var result = await _bannedSubredditService.UnbanSubreddit(Context.Guild.Id, subreddit);
            await ReplyAsync(result);
        }

        [Command("bans")]
        public async Task Bans()
        {
            var bannedSubreddits = (await _bannedSubredditService.GetBannedSubreddits(Context.Guild.Id)).ToList();
            var bannedSubredditsBuilder = new EmbedBuilder
            {
                Title = $"Banned subreddits in: {Context.Guild.Name}:",
                Fields = new List<EmbedFieldBuilder>(),
                Footer = new EmbedFooterBuilder
                {
                    Text = $"Total banned subreddits: {bannedSubreddits.Count}"
                }
            };

            var valueBuilder = new StringBuilder();
            foreach (var subredditDto in bannedSubreddits)
            {
                valueBuilder.Append(subredditDto.Name + "\n");
            }

            bannedSubredditsBuilder.Fields.Add(new EmbedFieldBuilder()
            {
                Name = "Subreddits",
                Value = valueBuilder.ToString()
            });

            await ReplyAsync("", false, bannedSubredditsBuilder.Build());
        }

        #endregion

        #region utils

        private bool IsMod()
        {
            return _guildModService.IsMod(Context.User.Id, Context.Guild.Id);
        }

        #endregion
    }
}
