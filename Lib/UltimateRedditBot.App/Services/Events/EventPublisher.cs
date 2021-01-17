using System;
using System.Linq;
using System.Threading.Tasks;

namespace UltimateRedditBot.App.Services.Events
{
    public class EventPublisher : IEventPublisher
    {
        #region Fields

        private readonly ISubscriptionService _subscriptionService;

        #endregion

        #region Constructor

        public EventPublisher(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        #endregion

        #region Methods

        public async Task Publish<T>(T eventMessage)
        {
            var subscribers = _subscriptionService.GetSubscriptions<T>().ToList();

            foreach (var subscriber in subscribers)
            {
                try
                {
                    await subscriber.HandleEvent(eventMessage);
                }
                catch (Exception e)
                {
                    //TODO Log the exception
                }
            }
        }

        #endregion
    }
}
