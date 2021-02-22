using System.Threading.Tasks;

namespace UltimateRedditBot.Discord.App.Services
{
    public interface ITextChannelSubscriptionService
    {

        Task<bool> IsSubscribed(int textChannelId, int subscriptionId);

        Task Subscribe(int textChannelId, int subscriptionId);

        Task Unsubscribe(int textChannelId, int subscriptionId);
    }
}
