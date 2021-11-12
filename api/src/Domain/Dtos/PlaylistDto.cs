
namespace Domain.Dtos;
public class PlaylistDto
{
    #region Properties

    public Guid Id { get; set; }

    public string Name { get; set; }

    public bool IsGlobal { get; set; }

    public Guid? GuildId { get; set; }

    public IEnumerable<SubredditDto> Subreddits { get; set; }

    #endregion

    #region Constructor

    public PlaylistDto()
    {
        Subreddits = new HashSet<SubredditDto>();
    }

    #endregion
}
