using System.Collections.Generic;
using System.Threading.Tasks;
using UltimateRedditBot.Domain.Enums;
using UltimateRedditBot.Domain.Models.Reddit;

namespace UltimateRedditBot.Infra.Services
{
    public interface IRedditSubscriptionService
    {
        Task<Subscription> GetSubscriptionById(int id);

        Task<IEnumerable<Subscription>> GetSubscriptionByIds(IEnumerable<int> ids);

        Task<Subscription> CreateAndGetSubscription(int subredditId, Sort sort);

        Task<Subscription> GetSubscriptionBySubredditAndSort(int subredditId, Sort sort);

        Task<List<Subscription>> GetSubscriptions();

        Task Update(IEnumerable<Subscription> subscriptions);
    }
}
