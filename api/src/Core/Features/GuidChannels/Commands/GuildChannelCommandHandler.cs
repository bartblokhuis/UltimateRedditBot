using AutoMapper;
using Core.Features.Common;
using Database;
using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.GuidChannels.Commands;

internal class GuildChannelCommandHandler : BaseCommandHandler, IRequestHandler<AddChannelCommand, Result<GuildChannelDto>>
{
    #region Constructor

    public GuildChannelCommandHandler(UltimateRedditBotDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    #endregion

    #region Methods

    public async Task<Result<GuildChannelDto>> Handle(AddChannelCommand command, CancellationToken cancellationToken)
    {
        var channel = _mapper.Map<GuildChannel>(command);
        await _context.GuildChannels.AddAsync(channel, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<GuildChannelDto>(channel);
        return await Result<GuildChannelDto>.SuccessAsync(dto);
    }

    #endregion
}

