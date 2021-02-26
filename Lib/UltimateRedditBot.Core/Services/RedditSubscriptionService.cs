using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UltimateRedditBot.Domain.Enums;
using UltimateRedditBot.Domain.Models.Reddit;
using UltimateRedditBot.Infra.BaseRepository;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Core.Services
{
    public class RedditSubscriptionService : IRedditSubscriptionService
    {
        #region Fields

        private readonly IBaseRepository<Subscription> _subscriptionRepo;

        #endregion

        #region Constructor

        public RedditSubscriptionService(IBaseRepository<Subscription> subscriptionRepo)
        {
            _subscriptionRepo = subscriptionRepo;
        }

        #endregion

        #region Methods

        public Task<Subscription> GetSubscriptionById(int id)
        {
            return _subscriptionRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionByIds(IEnumerable<int> ids)
        {
            var subscriptions = await _subscriptionRepo.Table.AsQueryable().Where(x => ids.Contains(x.Id)).ToListAsync();
            return subscriptions;
        }

        public async Task<Subscription> CreateAndGetSubscription(int subredditId, Sort sort)
        {
            var subscription = new Subscription
            {
                SubredditId = subredditId,
                Sort = sort,
                LastPostId = ""
            };

            await _subscriptionRepo.InsertAsync(subscription);
            return subscription;
        }

        public Task<Subscription> GetSubscriptionBySubredditAndSort(int subredditId, Sort sort)
        {
            return _subscriptionRepo.Table.AsQueryable().FirstOrDefaultAsync(x => x.SubredditId == subredditId && x.Sort == sort);
        }

        public Task<List<Subscription>> GetSubscriptions()
        {
            return _subscriptionRepo.Table.Include(x => x.Subreddit).ToListAsync();
        }

        public Task Update(IEnumerable<Subscription> subscriptions)
        {
            return _subscriptionRepo.UpdateRangeAsync(subscriptions);
        }

        #endregion
    }
}
