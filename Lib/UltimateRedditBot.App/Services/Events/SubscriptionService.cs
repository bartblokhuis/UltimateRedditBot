using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace UltimateRedditBot.App.Services.Events
{
    public class SubscriptionService : ISubscriptionService
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Constructor

        public SubscriptionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Methods

        public IEnumerable<IConsumer<T>> GetSubscriptions<T>()
        {
            return _serviceProvider.GetServices<IConsumer<T>>();
        }

        #endregion
    }
}
