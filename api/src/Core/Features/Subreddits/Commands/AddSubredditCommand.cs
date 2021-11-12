using Domain.Dtos;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Subreddits.Commands;

public class AddSubredditCommand : IRequest<Result<SubredditDto>>
{
    #region Properties

    public string Name { get; set; }

    public bool IsNsfw { get; set; }

    #endregion

    #region Constructor

    public AddSubredditCommand(string name, bool isNsfw)
    {
        Name = name;
        IsNsfw = isNsfw;
    }

    #endregion
}
