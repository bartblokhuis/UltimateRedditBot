using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Domain.Models.Reddit;
using UltimateRedditBot.Infra.BaseRepository;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Core.Services
{
    public class SubredditService : ISubredditService
    {
        #region Constructor

        public SubredditService(IBaseRepository<Subreddit> subredditRepo, IRedditApiService redditApiService,
            IMapper mapper)
        {
            _subredditRepo = subredditRepo;
            _redditApiService = redditApiService;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<SubredditDto> GetSubredditDtoByName(string subredditName)
        {
            var subreddit =
                await _subredditRepo.Table.FirstOrDefaultAsync(x => x.Name.ToLower() == subredditName.ToLower());

            SubredditDto subredditDto;
            if (subreddit != null)
            {
                subredditDto = _mapper.Map<SubredditDto>(subreddit);
            }
            else
            {
                subredditDto = await _redditApiService.GetSubredditByName(subredditName);
                if (subredditDto == null)
                    return null;

                subreddit = _mapper.Map<Subreddit>(subredditDto);
                await _subredditRepo.InsertAsync(subreddit);
            }

            return subredditDto;
        }

        #endregion

        #region Fields

        private readonly IBaseRepository<Subreddit> _subredditRepo;
        private readonly IRedditApiService _redditApiService;
        private readonly IMapper _mapper;

        #endregion
    }
}