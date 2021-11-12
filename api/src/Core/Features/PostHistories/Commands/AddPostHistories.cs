using Domain.Wrapper;
using MediatR;

namespace Core.Features.PostHistories.Commands
{
    public class AddPostHistories : IRequest<Result>
    {
        public IEnumerable<PostHistoryDto> PostHistories { get; set; }
    }
}
