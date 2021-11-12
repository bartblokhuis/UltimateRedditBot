using AutoMapper;
using Core.Consts;
using Core.Features.Common;
using Database;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Settings;
using Domain.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Guilds.Commands;

internal class GuildCommandHandler : BaseCommandHandler, IRequestHandler<CreateGuildCommand, Result<GuildDto>>, IRequestHandler<UpdateGuildSettingsCommand, Result<GuildDto>>, IRequestHandler<GuildLeaveCommand, Result>
{
    #region Fields

    private readonly LimitSettings _limitSettings;

    #endregion
    #region Constructor

    public GuildCommandHandler(UltimateRedditBotDbContext context, IMapper mapper, LimitSettings limitSettings)
        : base(context, mapper)
    {
        _limitSettings = limitSettings;
    }

    #endregion

    #region Methods

    public async Task<Result<GuildDto>> Handle(CreateGuildCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        if (string.IsNullOrEmpty(command.DiscordGuildId))
            throw new ArgumentNullException(nameof(command.DiscordGuildId));

        var guild = _mapper.Map<Guild>(command);

        guild.GuildSetting = new GuildSetting
        {
            AllowNsfw = true,
            Id = Guid.NewGuid(),
            IsEnabled = true,
            Prefix = "$",
            Sort = Sort.Hot,
            MaxPlaylists = _limitSettings.DefaultMaxPlaylists,
            MaxQueueItems = _limitSettings.DefaultMaxItemsInQueue,
            MaxSubscriptions = _limitSettings.DefaultMaxSubscriptions
        };

        guild.GuildSettingId = guild.GuildSetting.Id;

        await _context.Guilds.AddAsync(guild, cancellationToken);
        await _context.SaveChangesAsync();

        var mappedGuild = _mapper.Map<GuildDto>(guild);
        mappedGuild.Setting = _mapper.Map<GuildSettingDto>(guild.GuildSetting);
        return await Result<GuildDto>.SuccessAsync(mappedGuild);
    }

    public async Task<Result<GuildDto>> Handle(UpdateGuildSettingsCommand command, CancellationToken cancellationToken)
    {
        if(command == null)
            throw new ArgumentNullException(nameof(command));

        var guild = await _context.Guilds.Include(x => x.GuildSetting).FirstOrDefaultAsync(guild => guild.Id == command.GuildId, cancellationToken);
        if(guild == null)
            throw new ArgumentNullException(nameof(guild));

        var newGuildSettings = _mapper.Map<GuildSetting>(command);
        newGuildSettings.Id = guild.GuildSetting.Id;
        newGuildSettings.MaxQueueItems = guild.GuildSetting.MaxQueueItems;
        newGuildSettings.MaxPlaylists = guild.GuildSetting.MaxPlaylists;
        newGuildSettings.MaxSubscriptions = guild.GuildSetting.MaxSubscriptions;

        guild.GuildSetting = newGuildSettings;

        _context.Guilds.Update(guild);
         await _context.SaveChangesAsync(cancellationToken);

        var mappedGuild = _mapper.Map<GuildDto>(guild);
        mappedGuild.Setting = _mapper.Map<GuildSettingDto>(guild.GuildSetting);
        return await Result<GuildDto>.SuccessAsync(mappedGuild);
    }

    public async Task<Result> Handle(GuildLeaveCommand command, CancellationToken cancellationToken)
    {
        var guild = await _context.Guilds.Include(x => x.PostHistories).Include(x => x.GuildAdmins).Include(x => x.GuildPlaylists).Include(x => x.BannedSubreddits).Include(x => x.GuildSetting).Include(x => x.GuildChannels).Include(x => x.BannedSubreddits).FirstOrDefaultAsync(guild => guild.Id == command.GuildId, cancellationToken);
        if (guild == null)
            return (await Result.FailAsync("Guild not found")) as Result;

        _context.Guilds.Remove(guild);
        
        await _context.SaveChangesAsync(cancellationToken); 
        return (await Result.SuccessAsync()) as Result;
    }

    #endregion
}

