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
        #region Constructor

        public PostHistoryService(IBaseRepository<PostHistory, int, UltimateDiscordDbContext> postHistoryRepo,
            IMapper mapper)
        {
            _postHistoryRepo = postHistoryRepo;
            _mapper = mapper;
        }

        #endregion

        #region Fields

        private readonly IBaseRepository<PostHistory, int, UltimateDiscordDbContext> _postHistoryRepo;
        private readonly IMapper _mapper;

        #endregion

        #region Methods

        public async Task<PostHistory> GetPostHistory(bool isForGuild, ulong id, int subredditId)
        {
            var postHistory = isForGuild
                ? _postHistoryRepo.Table.FirstOrDefault(x => x.SubredditId == subredditId && x.GuildId == id)
                : _postHistoryRepo.Table.FirstOrDefault(x => x.SubredditId == subredditId && x.UserId == id);

            return postHistory;
        }

        public string GetPostHistoryName(bool isForGuild, ulong id, int subredditId)
        {
            return isForGuild
                ? _postHistoryRepo.Table.AsNoTracking()
                    .FirstOrDefault(x => x.SubredditId == subredditId && x.GuildId == id)?.PostId
                : _postHistoryRepo.Table.AsNoTracking()
                    .FirstOrDefault(x => x.SubredditId == subredditId && x.UserId == id)?.PostId;
        }

        public Task SavePostHistory(PostHistory postHistory)
        {
            return postHistory.Id == 0
                ? _postHistoryRepo.InsertAsync(postHistory)
                : _postHistoryRepo.SaveChanges();
        }

        public async Task ClearPostHistoryHistory(bool isForGuild, ulong id, int subredditId)
        {
            var postHistory = await GetPostHistory(isForGuild, id, subredditId);
            await _postHistoryRepo.DeleteAsync(postHistory);
        }

        #endregion
    }
}
