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

        Task<Subscription> CreateAndGetSubscription(ulong subredditId, Sort sort);
    }
}
