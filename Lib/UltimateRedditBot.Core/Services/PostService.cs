using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task SavePosts(IEnumerable<PostDto> postDtos)
        {
            var posts = _mapper.Map<IEnumerable<Post>>(postDtos).ToList();
            if (!posts.Any())
                return;

            //TODO replace to an add if not exists method.
            var newPostIds = posts.Select(x => x.Id).ToList();

            var existingPostsWithSameId =
                await _postRepository.Table.AsNoTracking().AsQueryable().Where(x => newPostIds.Contains(x.Id)).ToListAsync();

            posts = posts.Where(x => existingPostsWithSameId.All(y => x.Id != y.Id)).ToList();
            await _postRepository.InsertAsync(posts);
        }

        public async Task<PostDto> GetPostDtoById(string postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            return post != null ? _mapper.Map<PostDto>(post) : null;
        }

        public Task<List<Post>> GetPostDtoByIds(IEnumerable<string> postIds)
        {
            return _postRepository.Table.AsQueryable().Where(x => postIds.Contains(x.Id)).ToListAsync();
        }

        #endregion
    }
}
