namespace Domain.Dtos;

public class SubredditDto
{
    #region Properties

    public Guid Id { get; set; }

    public string Name { get; set; }

    public bool IsNsfw { get; set; }

    public bool IsBanned { get; set; }

    #endregion
}

