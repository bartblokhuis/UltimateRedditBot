using AutoMapper;
using Core.Features.Common;
using Database;
using Domain.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.PostHistories.Queries
{
    internal class PostHistoryQueryHandler : BaseQueryHandler, IRequestHandler<GetPostHistoryQuery, Result<PostHistoryDto>>
    {
        #region Constructor

        public PostHistoryQueryHandler(UltimateRedditBotDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        #endregion

        #region Methods

        public async Task<Result<PostHistoryDto>> Handle(GetPostHistoryQuery request, CancellationToken cancellationToken)
        {
            var postHistory = await _context.PostHistories.FirstOrDefaultAsync(history => history.SubredditId == request.SubredditId && history.GuildId == request.GuildId && history.Sort == request.Sort);
            if (postHistory == null)
                return await Result<PostHistoryDto>.FailAsync("Post history not found");

            var mappedPostHistory = _mapper.Map<PostHistoryDto>(postHistory);
            return await Result<PostHistoryDto>.SuccessAsync(mappedPostHistory);
        }

        #endregion
    }
}
