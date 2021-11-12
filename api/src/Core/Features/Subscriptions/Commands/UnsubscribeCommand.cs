﻿using Domain.Enums;
using Domain.Wrapper;
using MediatR;

namespace Core.Features.Subscriptions.Commands;

public class UnSubscribeCommand : IRequest<Result<Guid>>
{
    #region Properties

    public Guid SubredditId { get; set; }

    public Sort Sort { get; set; }

    public Guid ChannelId { get; set; }

    #endregion
}
