using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UltimateRedditBot.Discord.Domain.Models;

namespace UltimateRedditBot.Discord.App.Services
{
    public interface ITextChannelSubscriptionService
    {

        Task<bool> IsSubscribed(int textChannelId, int subscriptionId);

        Task Subscribe(int textChannelId, int subscriptionId);

        Task Unsubscribe(TextChannelSubscription subscription);

        Task<List<TextChannelSubscription>> GetTextChannelSubscriptions();

        Task<TextChannelSubscription> GetTextChannelSubscription(int channelId, int subscriptionId);
    }
}
