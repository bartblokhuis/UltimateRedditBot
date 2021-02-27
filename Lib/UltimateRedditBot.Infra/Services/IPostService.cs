using System.Collections.Generic;
using System.Threading.Tasks;
using UltimateRedditBot.Domain.Dtos.Reddit;

namespace UltimateRedditBot.Infra.Services
{
    public interface IPostService
    {
        Task SavePost(PostDto post);

        Task SavePosts(IEnumerable<PostDto> postDtos);

        Task<PostDto> GetPostDtoById(string postId);
    }
}
