using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UltimateRedditBot.Discord.Database;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Infra.BaseRepository;

namespace UltimateRedditBot.Discord.App.Services
{
    public class PostHistoryService : IPostHistoryService
    {
        #region Fields

        private readonly IBaseRepository<PostHistory, int, UltimateDiscordDbContext> _postHistoryRepo;

        #endregion

        #region Constructor

        public PostHistoryService(IBaseRepository<PostHistory, int, UltimateDiscordDbContext> postHistoryRepo)
        {
            _postHistoryRepo = postHistoryRepo;
        }

        #endregion

        #region Methods

        public PostHistory GetPostHistory(bool isForGuild, ulong id, int subredditId)
        {
            var postHistory = isForGuild
                ? _postHistoryRepo.Table.FirstOrDefault(x => x.SubredditId == subredditId && x.GuildId == id)
                : _postHistoryRepo.Table.FirstOrDefault(x => x.SubredditId == subredditId && x.UserId == id);

            return postHistory;
        }

        public PostHistory GetPostHistoryPost(bool isForGuild, ulong id, int subredditId)
        {
            return isForGuild
                ? _postHistoryRepo.Table.AsNoTracking()
                    .FirstOrDefault(x => x.SubredditId == subredditId && x.GuildId == id)
                : _postHistoryRepo.Table.AsNoTracking()
                    .FirstOrDefault(x => x.SubredditId == subredditId && x.UserId == id);
        }

        public Task SavePostHistory(PostHistory postHistory)
        {
            return postHistory.Id == 0
                ? _postHistoryRepo.InsertAsync(postHistory)
                : _postHistoryRepo.SaveChanges();
        }

        public async Task ClearPostHistory(bool isForGuild, ulong id, int subredditId)
        {
            var postHistory = GetPostHistory(isForGuild, id, subredditId);
            if (postHistory == null)
                return;

            await _postHistoryRepo.DeleteAsync(postHistory);
        }

        #endregion
    }
}
