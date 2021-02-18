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

        public async Task<Subscription> CreateAndGetSubscription(ulong subredditId, Sort sort)
        {
            var subscription = new Subscription
            {
                SubredditId = subredditId,
                Sort = sort
            };

            await _subscriptionRepo.InsertAsync(subscription);
            return subscription;
        }

        #endregion
    }
}
