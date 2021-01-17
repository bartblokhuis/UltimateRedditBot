using System.Threading.Tasks;
using UltimateRedditBot.Core.Constants;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.App.Services.Queue
{
    public class QueueService : IQueueService
    {
        #region Fields

        private readonly IQueueManager _queueManager;
        private readonly IGenericSettingService _genericSettingService;

        #endregion

        #region Constructor

        public QueueService(IQueueManager queueManager, IGenericSettingService genericSettingService)
        {
            _queueManager = queueManager;
            _genericSettingService = genericSettingService;
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
        public async Task<string> AddToQueue(string group, ulong id, string subreddit, int amountOfTimes)
        {
            var max = await _genericSettingService.GetSettingValueByKeyGroupAndKey<int>(group,
                GenericSettingKeyConstants.BulkSettingKey);

            if (max > amountOfTimes)
            {
                amountOfTimes = max;
            }

            return string.Empty;
        }

        #endregion
    }
}
