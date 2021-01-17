using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using UltimateRedditBot.Infra.Services;

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
