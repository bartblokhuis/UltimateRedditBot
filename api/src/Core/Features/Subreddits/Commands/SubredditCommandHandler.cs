using AutoMapper;
using Core.Features.Common;
using Database;
using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Subreddits.Commands;

internal class SubredditCommandHandler : BaseCommandHandler, IRequestHandler<AddSubredditCommand, Result<SubredditDto>>
{
    #region Constructor

    public SubredditCommandHandler(UltimateRedditBotDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    #endregion

    #region Methods

    public async Task<Result<SubredditDto>> Handle(AddSubredditCommand command, CancellationToken cancellationToken)
    {
        if(command == null)
            throw new ArgumentNullException(nameof(command));

        if (string.IsNullOrEmpty(command.Name))
            throw new ArgumentNullException(nameof(command.Name));

        var subreddit = _mapper.Map<Subreddit>(command);
        await _context.Subreddits.AddAsync(subreddit, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var mappedSubreddit = _mapper.Map<SubredditDto>(subreddit);
        return await Result<SubredditDto>.SuccessAsync(mappedSubreddit);
    }

    #endregion
}
