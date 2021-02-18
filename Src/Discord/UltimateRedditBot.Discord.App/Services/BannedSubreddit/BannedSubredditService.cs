using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UltimateRedditBot.Discord.Database;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Domain.Models.Reddit;
using UltimateRedditBot.Infra.BaseRepository;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Discord.App.Services
{
    public class BannedSubredditService : IBannedSubredditService
    {
        #region Fields

        private readonly IBaseRepository<BannedSubreddit, int, UltimateDiscordDbContext>
            _bannedSubredditRepo;

        private readonly ISubredditService _subredditService;
        #endregion

        #region Constructor

        public BannedSubredditService(IBaseRepository<Domain.Models.BannedSubreddit, int, UltimateDiscordDbContext> bannedSubredditRepo, ISubredditService subredditService)
        {
            _bannedSubredditRepo = bannedSubredditRepo;
            _subredditService = subredditService;
        }

        #endregion

        #region Methods

        public async Task<string> BanSubreddit(ulong id, string subredditName)
        {
            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            if (subreddit == null)
                return "Subreddit doesnt exist";

            if (IsSubredditBanned(id, subreddit.Id))
                return "Subreddit is already banned";

            await _bannedSubredditRepo.InsertAsync(new Domain.Models.BannedSubreddit
            {
                GuildId = id,
                SubredditId = subreddit.Id
            });

            return $"Banned the {subredditName} subreddit.";
        }

        public async Task<bool> IsSubredditBanned(ulong id, string subredditName)
        {
            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            return subreddit != null && IsSubredditBanned(id, subreddit.Id);
        }

        public bool IsSubredditBanned(ulong id, SubredditDto subreddit)
        {
            return subreddit != null && IsSubredditBanned(id, subreddit.Id);
        }

        public async Task<string> UnbanSubreddit(ulong id, string subredditName)
        {
            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            if (subreddit == null)
                return "Subreddit doesnt exist";

            var banRecord = _bannedSubredditRepo.Table.FirstOrDefault(x => x.GuildId == id && x.SubredditId == subreddit.Id);
            if(banRecord == null)
                return "Subreddit is not banned";

            await _bannedSubredditRepo.DeleteAsync(banRecord);
            return $"Unbanned the {subredditName} subreddit";
        }

        public Task<List<int>> GetBannedSubredditIds(ulong guildId)
        {
            return _bannedSubredditRepo.Table.AsQueryable().Where(x => x.GuildId == guildId)?
                .Select(x => x.SubredditId).ToListAsync();
        }

        public async Task<IEnumerable<SubredditDto>> GetBannedSubreddits(ulong guildId)
        {
            var bannedSubredditIds = await GetBannedSubredditIds(guildId);
            return await _subredditService.GetAllByIds(bannedSubredditIds);
        }

        #endregion

        #region Utils

        private bool IsSubredditBanned(ulong id, int subredditId)
        {
            return _bannedSubredditRepo.Table.Any(x => x.GuildId == id && x.SubredditId == subredditId);
        }

        #endregion
    }
}
