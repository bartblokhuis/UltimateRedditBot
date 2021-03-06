using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.Domain.Dtos.Reddit;
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
        private readonly IPostService _postService;
        private DateTime _lastCheckRemovedPosts = DateTime.Now.AddMinutes(-1);

        #endregion

        #region Constructor

        public RedditSubscriptionHostedService(IRedditSubscriptionService redditSubscriptionService, IRedditApiService redditApiService, ILogger<RedditSubscriptionHostedService> logger, IEventPublisher eventPublisher, IPostService postService)
        {
            _redditSubscriptionService = redditSubscriptionService;
            _redditApiService = redditApiService;
            _logger = logger;
            _eventPublisher = eventPublisher;
            _postService = postService;

            _logger.LogInformation("Starting subscription service");
        }

        #endregion

        #region Methods

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var minutesPassed = (DateTime.Now - _lastCheckRemovedPosts).TotalMinutes;
                if(minutesPassed >= 5)
                {
                    await CheckForRemovedPosts();
                    _lastCheckRemovedPosts = DateTime.Now;
                }

                await HandleSubscriptions(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
            }

            await Task.Delay(10000);

            if (cancellationToken.IsCancellationRequested)
                return;

            StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        private async Task CheckForRemovedPosts()
        {
            var subscriptions = await _redditSubscriptionService.GetSubscriptions();

            var subscriptionsToUpdate = new List<Subscription>();
            foreach (var subscription in subscriptions)
            {
                if (!await _redditApiService.IsPostRemoved(subscription.Post.PostLink))
                    continue;

                _logger.LogInformation($"Found a removed post in subreddit: {subscription.Subreddit.Name}, ");

                subscription.Post = null;
                subscription.PostId = null;

                subscriptionsToUpdate.Add(subscription);
            }

            if (subscriptionsToUpdate.Any())
                await _redditSubscriptionService.Update(subscriptionsToUpdate);
        }

        private async Task HandleSubscriptions(CancellationToken cancellationToken)
        {
            var allSubscriptions = await _redditSubscriptionService.GetSubscriptions();

            if (!allSubscriptions.Any())
                return;

            var postRequests = allSubscriptions.Select(x => GetSubscriptionPost(x)).ToList();

            await Task.WhenAll(postRequests);

            postRequests = postRequests.Where(x => x.Result?.PostDto != null).ToList();

            if (postRequests.All(x => x.Result == null))
                return;

            var posts = new List<PostDto>();

            //Update last post ids
            foreach (var postRequest in postRequests)
            {
                var post = postRequest.Result.PostDto;
                post.SubRedditId = postRequest.Result.Subscription.SubredditId;

                if(posts.Where(x => x.SubRedditId != 0).All(x => x.Id != post.Id))
                    posts.Add(post);

                postRequest.Result.Subscription.PostId = postRequest.Result.PostDto.Id;
            }

            await _postService.SavePosts(posts);

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
            var post = await _redditApiService.GetSubscriptionPost(subscription.Subreddit.Name, subscription.PostId,
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
