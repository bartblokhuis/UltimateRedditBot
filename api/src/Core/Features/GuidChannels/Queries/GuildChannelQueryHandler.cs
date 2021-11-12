using AutoMapper;
using Core.Features.Common;
using Core.Features.GuidChannels.Commands;
using Database;
using Domain.Dtos;
using Domain.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.GuidChannels.Queries
{
    internal class GuildChannelQueryHandler : BaseQueryHandler, IRequestHandler<GetChannelByDiscordIdQuery, Result<GuildChannelDto>>
    {
        #region Fields

        private readonly IMediator _mediator;

        #endregion

        #region Constructor

        public GuildChannelQueryHandler(UltimateRedditBotDbContext context, IMapper mapper, IMediator mediator) : base(context, mapper)
        {
            _mediator = mediator;
        }

        #endregion

        #region Methods

        public async Task<Result<GuildChannelDto>> Handle(GetChannelByDiscordIdQuery request, CancellationToken cancellationToken)
        {
            var channel = await _context.GuildChannels.FirstOrDefaultAsync(channel => channel.DiscordChannelId == request.DiscordChannelId, cancellationToken);
            if (channel == null)
            {
                //If the channel doesn't exist yet we register the user here.
                var addChannelCommand = new AddChannelCommand
                {
                    DiscordChannelId = request.DiscordChannelId,
                    GuildId = request.GuildId
                };

                return await _mediator.Send(addChannelCommand);
            }

            var mappedChannel = _mapper.Map<GuildChannelDto>(channel);
            return await Result<GuildChannelDto>.SuccessAsync(mappedChannel);
        }

        #endregion


    }
}
