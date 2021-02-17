using System.Threading.Tasks;
using UltimateRedditBot.Domain.Models.Reddit;

namespace UltimateRedditBot.Infra.Services
{
    public interface ISubscriptionService
    {
        Task<Subscription> GetSubscription();

        Task Subscripbe();

        Task Unsubscribe();
    }
}
