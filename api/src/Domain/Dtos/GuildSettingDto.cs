using Domain.Enums;

namespace Domain.Dtos;

public class GuildSettingDto
{
    #region Properties

    public string Prefix { get; set; }

    public bool AllowNsfw { get; set; }

    public bool IsEnabled { get; set; }

    public int MaxQueueItems { get; set; }

    public int MaxPlaylists { get; set; }

    public int MaxSubscriptions { get; set; }

    public Sort Sort { get; set; }

    #endregion
}

