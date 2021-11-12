using AutoMapper;
using Core.Features.Guilds.Commands;
using Database;
using Domain.Dtos;
using Domain.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Guilds.Queries;

internal class GuildQueryHandler : IRequestHandler<GetGuildByDiscordIdQuery, Result<GuildDto>>,
    IRequestHandler<IsSubredditBannedQuery, Result<bool>>, IRequestHandler<GetGuildsQuery, ListResult<GuildDto>>, IRequestHandler<GetGuildsCountQuery, Result<int>>
{
    #region Fields

    private readonly UltimateRedditBotDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    #endregion

    #region Constructor

    public GuildQueryHandler(UltimateRedditBotDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    #endregion

    #region Methods

    public async Task<Result<GuildDto>> Handle(GetGuildByDiscordIdQuery request, CancellationToken cancellationToken)
    {
        var guild = await _context.Guilds.Include(guild => guild.GuildSetting).FirstOrDefaultAsync(guild => guild.DiscordGuildId == request.DiscordId, cancellationToken);
        if(guild == null)
        {
            //If the guild doesn't exist yet we register it here.
            var createGuildCommand = new CreateGuildCommand
            {
                DiscordGuildId = request.DiscordId,
            };

            return await _mediator.Send(createGuildCommand);
        }

        var mappedGuild = _mapper.Map<GuildDto>(guild);
        mappedGuild.Setting = _mapper.Map<GuildSettingDto>(guild.GuildSetting);
        return await Result<GuildDto>.SuccessAsync(mappedGuild);
    }

    public async Task<Result<bool>> Handle(IsSubredditBannedQuery request, CancellationToken cancellationToken)
    {
        if(request == null)
            throw new NullReferenceException(nameof(request));

        var guild = await _context.Guilds.Include(guild => guild.BannedSubreddits).FirstOrDefaultAsync(guild => guild.Id == request.GuildId, cancellationToken);
        
        if (guild == null)
            throw new ArgumentException(nameof(guild));

        var isBanned = guild.BannedSubreddits.Any(bannedSubreddit => bannedSubreddit.SubredditId == request.SubredditId);
        return Result<bool>.Success(isBanned);
    }

    public async Task<ListResult<GuildDto>> Handle(GetGuildsQuery request, CancellationToken cancellationToken)
    {
        var guilds = await _context.Guilds.ToListResultAsync(cancellationToken);
        if (!guilds.Data.Any())
            return await ListResult<GuildDto>.FailAsync("No guilds found") as ListResult<GuildDto>;

        var mappedGuilds = _mapper.Map<ListResult<GuildDto>>(guilds);
        return mappedGuilds;
    }

    public async Task<Result<int>> Handle(GetGuildsCountQuery request, CancellationToken cancellationToken)
    {
        var guildsCount = await _context.Guilds.CountAsync(cancellationToken);
        return await Result<int>.SuccessAsync(guildsCount);
    }

    #endregion

    #region Utils

    #endregion
}

