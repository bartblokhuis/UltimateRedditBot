using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using UltimateRedditBot.Domain.Dtos.Reddit;
using UltimateRedditBot.Domain.Models.Reddit;
using UltimateRedditBot.Infra.BaseRepository;
using UltimateRedditBot.Infra.Services;

namespace UltimateRedditBot.Core.Services
{
    public class PostService : IPostService
    {
        #region Fields

        private readonly IBaseRepository<Post, string> _postRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public PostService(IBaseRepository<Post, string> postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public Task SavePost(PostDto post)
        {
            var posts = _mapper.Map<Post>(post);
            return posts != null ? _postRepository.InsertAsync(posts) : Task.CompletedTask;
        }

        public Task SavePosts(IEnumerable<PostDto> postDtos)
        {
            var posts = _mapper.Map<IEnumerable<Post>>(postDtos);
            return posts != null ? _postRepository.InsertAsync(posts) : Task.CompletedTask;
        }

        #endregion
    }
}
