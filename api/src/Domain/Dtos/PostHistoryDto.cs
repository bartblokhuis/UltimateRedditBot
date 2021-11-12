using Domain.Enums;

public class PostHistoryDto
{
    #region Properties

    public string LastPostId { get; set; }

    public Sort Sort { get; set; }

    public Guid SubredditId { get; set; }

    public Guid GuildId { get; set; }

    #endregion
}

