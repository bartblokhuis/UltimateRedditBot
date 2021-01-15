using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UltimateRedditBot.Core.Constants;
using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Core.Services
{
    public class RedditApiService : IRedditApiService
    {
        #region Fields

        private readonly IHttpClientFactory _clientFactory;

        #endregion

        #region Constructor

        public RedditApiService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        #endregion

        #region Methods

        public async Task<SubredditDto> GetSubredditByName(string subredditName)
        {
            var client = _clientFactory.CreateClient();
            var request = await client.GetAsync(string.Format(RedditApiConstants.SearchSubRedditByNameUrl, subredditName));

            if (!request.IsSuccessStatusCode)
            {
                return null;
            }

            var responseBody = await request.Content.ReadAsStringAsync();
            client.Dispose();

            return ParseSubReddit(subredditName, responseBody);
        }

        #endregion

        #region Utils

        private static SubredditDto ParseSubReddit(string name, string responseData)
        {
            dynamic data = JObject.Parse(responseData);

            try
            {
                var baseElement = data.data.children[0].data;
                var displayName = (string)baseElement.display_name;

                if (!ValidSubReddit(name, displayName))
                    return null;

                var isOver18 = (bool)baseElement.over18;

                return new SubredditDto(name, isOver18);

            }
            catch (Exception e)
            {
            }

            return null;
        }

        private static bool ValidSubReddit(string name, string responseName)
            => name.Equals(responseName, StringComparison.OrdinalIgnoreCase);

        #endregion
    }
}
