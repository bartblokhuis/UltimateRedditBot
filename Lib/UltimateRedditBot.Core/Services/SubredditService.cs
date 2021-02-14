using System;
using System.Collections.Generic;
using System.Linq;
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
        #region Fields

        private readonly IBaseRepository<Subreddit> _subredditRepo;
        private readonly IRedditApiService _redditApiService;
        private readonly IMapper _mapper;

        #endregion

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

        public async Task<string> GetRandomSubredditName(bool isOver18, IEnumerable<int> bannedSubredditIds = null)
        {
            bannedSubredditIds ??= new List<int>();
            bannedSubredditIds = bannedSubredditIds.ToList();

            var subreddits = _subredditRepo.Table.AsQueryable().Where(x => !bannedSubredditIds.Contains(x.Id));

            if (!isOver18)
                subreddits = subreddits.Where(x => !x.IsNsfw);

            var amountOfMappedSubreddits = await subreddits.CountAsync();

            var randomGen = new Random();
            var sub = randomGen.Next(amountOfMappedSubreddits);

           return subreddits.Skip(sub -1).First().Name;
        }

        #endregion
    }
}
