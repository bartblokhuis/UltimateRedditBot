using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UltimateRedditBot.Core.Constants;
using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Domain.Enums;
using UltimateRedditBot.Domain.Models.Reddit;
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

        public async Task<PostDto> GetOldPost(string subRedditName, string previousName, Sort sort, PostType postType, Guid id)
        {
            var post = await Process(1, subRedditName, "after", previousName, sort, postType);
            if (post is null)
                return null;
            return post;
        }

        private async Task<PostDto> Process(int maximumAttempts, string subRedditName, string beforeOrAfter, string previousName, Sort sort, PostType postType = PostType.Image)
        {
            for (var i = 0; i < maximumAttempts; i++)
            {
                var url = string.Format(RedditApiConstants.GetRedditPostBase, subRedditName, sort.ToString().ToLowerInvariant(), beforeOrAfter, previousName);
                var client = _clientFactory.CreateClient();

                try
                {
                    var request = await client.GetAsync(url);
                    request.EnsureSuccessStatusCode();

                    var responseBody = await request.Content.ReadAsStringAsync();
                    var post = ProcessRequest(responseBody, out previousName, postType);

                    if (post is not null)
                        return post;
                }
                catch (Exception e)
                {
                    //_logger.LogError($"Error getting new post. {url}, {e.Message}");
                }
            }

            //If we got this far we are not able to find any posts.
            return null;
        }

        #endregion

        #region Utils

        private static PostDto ProcessRequest(string request, out string previousPostName, PostType postType)
        {
            previousPostName = string.Empty;

            var posts = ParseRequest(request).ToList();
            if (!posts.Any())
                return null;

            var post = postType switch
            {
                PostType.Image => posts.FirstOrDefault(x =>
                    x.GetPostType() == PostType.Gif || x.GetPostType() == PostType.Image ||
                    x.GetPostType() == PostType.Video),
                PostType.Gif => posts.FirstOrDefault(x => x.GetPostType() == PostType.Gif),
                PostType.Video => posts.FirstOrDefault(x => x.GetPostType() == PostType.Video),
                PostType.Post => posts.FirstOrDefault(x => x.GetPostType() == PostType.Post),
                _ => null
            };

            previousPostName = posts.Last().Id;
            return post;
        }

        private static IEnumerable<PostDto> ParseRequest(string request)
        {
            var apiPosts = JsonConvert.DeserializeObject<RedditApiPostDto>(request);

            var posts = apiPosts?.Data?.Children?.Select(x => x.Data)
                .Select(x =>
                {
                    var thumbsUp = int.Parse(x.Ups);

                    return new PostDto
                    {
                        Author = x.Author, Id = x.Name, IsOver18 = x.Over18, Selftext = x.Selftext,
                        Thumbnail = x.Thumbnail, Title = x.Title, PostLink = x.Permalink, Url = x.Url, Ups = thumbsUp
                    };
                });

            return posts;
        }

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
