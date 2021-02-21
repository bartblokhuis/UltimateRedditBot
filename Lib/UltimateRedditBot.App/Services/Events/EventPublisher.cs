using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace UltimateRedditBot.App.Services.Events
{
    public class EventPublisher : IEventPublisher
    {
        #region Fields

        private readonly ISubscriptionService _subscriptionService;
        private readonly ILogger<EventPublisher> _logger;

        #endregion

        #region Constructor

        public EventPublisher(ISubscriptionService subscriptionService, ILogger<EventPublisher> logger)
        {
            _subscriptionService = subscriptionService;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task Publish<T>(T eventMessage)
        {
            var subscribers = _subscriptionService.GetSubscriptions<T>().ToList();

            foreach (var subscriber in subscribers)
                try
                {
                    await subscriber.HandleEvent(eventMessage);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
        }

        #endregion
    }
}
