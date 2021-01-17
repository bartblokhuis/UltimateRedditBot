using System.Collections.Generic;

namespace UltimateRedditBot.App.Services.Events
{
    public interface ISubscriptionService
    {
        IEnumerable<IConsumer<T>> GetSubscriptions<T>();
    }
}
