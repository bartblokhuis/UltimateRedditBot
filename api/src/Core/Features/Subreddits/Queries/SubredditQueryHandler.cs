using AutoMapper;
using Core.Features.Common;
using Database;
using Domain.Dtos;
using Domain.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Subreddits.Queries;

internal class SubredditQueryHandler : BaseQueryHandler, IRequestHandler<GetSubredditByNameQuery, Result<SubredditDto>>, IRequestHandler<GetSubredditsQuery, ListResult<SubredditDto>>, IRequestHandler<GetSubredditsCountQuery, Result<int>>
{
    #region Constructor

    public SubredditQueryHandler(UltimateRedditBotDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    #endregion

    #region Methods

    public async Task<Result<SubredditDto>> Handle(GetSubredditByNameQuery request, CancellationToken cancellationToken)
    {
        if(request == null)
            throw new ArgumentNullException(nameof(request));

        if(string.IsNullOrEmpty(request.Name))
            throw new ArgumentNullException(nameof(request.Name));

        var subreddit = await _context.Subreddits.FirstOrDefaultAsync(subreddit => subreddit.Name == request.Name.ToLower(), cancellationToken);
        if(subreddit == null)
            //Subreddit not found in database
            return await Result<SubredditDto>.FailAsync("Subreddit was not found in the database");

        var mappedSubreddit = _mapper.Map<SubredditDto>(subreddit);
        return await Result<SubredditDto>.SuccessAsync(mappedSubreddit);
    }

    public async Task<ListResult<SubredditDto>> Handle(GetSubredditsQuery request, CancellationToken cancellationToken)
    {
        var subreddits = await _context.Subreddits.ToListResultAsync(cancellationToken);
        if (!subreddits.Data.Any())
            return await ListResult<SubredditDto>.FailAsync("No subreddits found") as ListResult<SubredditDto>;

        var mappedSubreddits = _mapper.Map<ListResult<SubredditDto>>(subreddits);
        return mappedSubreddits;

    }

    public async Task<Result<int>> Handle(GetSubredditsCountQuery request, CancellationToken cancellationToken)
    {
        var subredditsCount = await _context.Subreddits.CountAsync(cancellationToken);
        return await Result<int>.SuccessAsync(subredditsCount);
    }

    #endregion

}
