using System.Threading.Tasks;
using UltimateRedditBot.App.Services.Events;
using UltimateRedditBot.Core.Constants;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.App.Services.Queue
{
    public class QueueService : IQueueService
    {
        #region Fields

        private readonly IQueueManager _queueManager;
        private readonly IGenericSettingService _genericSettingService;
        private readonly ISubredditService _subredditService;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Constructor

        public QueueService(IQueueManager queueManager, IGenericSettingService genericSettingService, ISubredditService subredditService, IEventPublisher eventPublisher)
        {
            _queueManager = queueManager;
            _genericSettingService = genericSettingService;
            _subredditService = subredditService;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="group"></param>
        /// <param name="id"></param>
        /// <param name="subreddit"></param>
        /// <param name="amountOfTimes">The amount of times a subreddit post is being requested</param>
        /// <returns></returns>
        public async Task<string> AddToQueue(string group, ulong id, string subredditName, int amountOfTimes)
        {
            var max = await _genericSettingService.GetSettingValueByKeyGroupAndKey<int>(group,
                GenericSettingKeyConstants.BulkSettingKey, id.ToString());

            if (max > amountOfTimes)
                amountOfTimes = max;

            var subreddit = await _subredditService.GetSubredditDtoByName(subredditName);
            await _eventPublisher.Publish(subreddit);
            if (subreddit == null)
                return "Subreddit doesn't exist";

            return string.Empty;
        }

        #endregion
    }
}
