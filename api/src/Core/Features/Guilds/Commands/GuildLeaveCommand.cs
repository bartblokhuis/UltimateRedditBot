using Domain.Wrapper;
using MediatR;

namespace Core.Features.Guilds.Commands
{
    public class GuildLeaveCommand : IRequest<Result>
    {
        public Guid GuildId { get; set; }
    }
}
