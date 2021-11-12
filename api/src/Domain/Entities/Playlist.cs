using Domain.Entities.Common;

namespace Domain.Entities;

public class Playlist : BaseEntity
{
    public string Name { get; set; }

    public bool IsGlobal { get; set; }

    public Guid? GuildId { get; set; }

    public Guild? Guild { get; set; }

    public ICollection<Subreddit> Subreddits { get; set; }

    public Playlist()
    {
        Subreddits = new HashSet<Subreddit>();

    }
}
