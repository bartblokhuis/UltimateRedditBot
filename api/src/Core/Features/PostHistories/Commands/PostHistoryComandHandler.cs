using AutoMapper;
using Core.Features.Common;
using Database;
using Domain.Entities;
using Domain.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.PostHistories.Commands;

public class PostHistoryComandHandler : BaseCommandHandler, IRequestHandler<AddPostHistories, Result>
{
    #region Constructor

    public PostHistoryComandHandler(UltimateRedditBotDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    #endregion

    #region Fields

    public async Task<Result> Handle(AddPostHistories command, CancellationToken cancellationToken)
    {
        if(command == null) 
            throw new ArgumentNullException(nameof(command));

        foreach (var history in command.PostHistories)
        {
            var oldHistory = await _context.PostHistories.FirstOrDefaultAsync(x => x.GuildId == history.GuildId && x.SubredditId == history.SubredditId && x.Sort == history.Sort, cancellationToken);
            if(oldHistory == null)
            {
                var postHistory = _mapper.Map<PostHistory>(history);
                await _context.PostHistories.AddAsync(postHistory, cancellationToken);
                continue;
            }

            oldHistory.LastPostId = history.LastPostId;
            _context.Update(oldHistory);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new Result() { Succeeded = true };
    }

    #endregion
}
