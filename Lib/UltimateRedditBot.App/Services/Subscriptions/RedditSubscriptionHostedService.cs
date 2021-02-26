using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.Domain.Enums;
using UltimateRedditBot.Domain.Models.Reddit;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.App.Services.Subscriptions
{
    public class RedditSubscriptionHostedService : IHostedService
    {
        #region Fields

        private readonly IRedditSubscriptionService _redditSubscriptionService;
        private readonly IRedditApiService _redditApiService;
        private readonly ILogger<RedditSubscriptionHostedService> _logger;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Constructor

        public RedditSubscriptionHostedService(IRedditSubscriptionService redditSubscriptionService, IRedditApiService redditApiService, ILogger<RedditSubscriptionHostedService> logger, IEventPublisher eventPublisher)
        {
            _redditSubscriptionService = redditSubscriptionService;
            _redditApiService = redditApiService;
            _logger = logger;
            _eventPublisher = eventPublisher;

            _logger.LogInformation("Starting subscription service");
        }

        #endregion

        #region Methods

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await HandleSubscriptions(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }

            await Task.Delay(1000);

            if (cancellationToken.IsCancellationRequested)
                return;

            StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        private async Task HandleSubscriptions(CancellationToken cancellationToken)
        {
            var allSubscriptions = await _redditSubscriptionService.GetSubscriptions();

            if (!allSubscriptions.Any())
                return;

            var postRequests = allSubscriptions.Select(x => GetSubscriptionPost(x)).ToList();

            await Task.WhenAll(postRequests);

            postRequests = postRequests.Where(x => x?.Result?.PostDto != null).ToList();

            if (postRequests.All(x => x.Result == null))
                return;

            //Update last post ids
            foreach (var postRequest in postRequests)
                postRequest.Result.Subscription.LastPostId = postRequest.Result.PostDto.Id;

            await _redditSubscriptionService.Update(postRequests.Select(x => x.Result.Subscription));

            try
            {
                await _eventPublisher.Publish(postRequests.Select(x => x.Result));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        #region Utils

        private async Task<SubscriptionPost> GetSubscriptionPost(Subscription subscription)
        {
            var post = await _redditApiService.GetSubscriptionPost(subscription.Subreddit.Name, subscription.LastPostId,
                subscription.Sort, PostType.Image, Guid.Empty);

            if (post != null)
            {
                return new SubscriptionPost
                {
                    Subscription = subscription,
                    PostDto = post
                };
            }

            return await Task.FromResult<SubscriptionPost>(null);
        }

        #endregion

        #endregion
    }
}
