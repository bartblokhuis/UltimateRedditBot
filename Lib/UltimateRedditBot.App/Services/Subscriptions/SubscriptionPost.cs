using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Domain.Models.Reddit;

namespace UltimateRedditBot.App.Services.Subscriptions
{
    public class SubscriptionPost
    {
        public Subscription Subscription { get; set; }

        public PostDto PostDto { get; set; }
    }
}
