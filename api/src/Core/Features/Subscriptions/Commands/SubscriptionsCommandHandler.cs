using AutoMapper;
using Core.Features.Common;
using Core.Features.Subscriptions.Queries;
using Database;
using Domain.Dtos;
using Domain.Entities;
using Domain.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Subscriptions.Commands;

internal class SubscriptionsCommandHandler : BaseCommandHandler, IRequestHandler<SubscribeCommand, Result<SubscriptionDto>>, IRequestHandler<UnSubscribeCommand, Result<Guid>>, IRequestHandler<UpdatePostIdCommand, Result>
{
    #region Fields

    private readonly IMediator _mediator;

    #endregion

    #region Constructor

    public SubscriptionsCommandHandler(UltimateRedditBotDbContext context, IMapper mapper, IMediator mediator) : base(context, mapper)
    {
        _mediator = mediator;
    }

    #endregion

    #region Methods

    public async Task<Result<SubscriptionDto>> Handle(SubscribeCommand command, CancellationToken cancellationToken)
    {
        var subscription = await _context.Subscriptions.Include(x => x.Subreddit)
            .FirstOrDefaultAsync(subscription => subscription.SubredditId == command.SubredditId && subscription.Sort == command.Sort, cancellationToken);

        if (subscription == null)
        {
            subscription = _mapper.Map<Subscription>(command);
            await _context.Subscriptions.AddAsync(subscription, cancellationToken);

            subscription.Subreddit = await _context.Subreddits.FirstOrDefaultAsync(x => x.Id == command.SubredditId, cancellationToken);
        }

        var textChannel = await _context.GuildChannels.Include(x => x.ChannelSubscriptions).FirstOrDefaultAsync(guildChannel => guildChannel.Id == command.ChannelId, cancellationToken);
        if (textChannel == null)
            throw new ArgumentNullException(nameof(textChannel));

        if (textChannel.ChannelSubscriptions.Any(channelSubscripiton => channelSubscripiton.SubscriptionId == subscription.Id))
            return await Result<SubscriptionDto>.FailAsync("Already subscribed!");

        var guild = await _context.Guilds.Include(x => x.GuildSetting).Include(x => x.GuildChannels).ThenInclude(x => x.ChannelSubscriptions).FirstOrDefaultAsync(x => x.Id == textChannel.GuildId, cancellationToken);
        var count = guild.GuildChannels.SelectMany(x => x.ChannelSubscriptions).Count();

        if (count >= guild.GuildSetting.MaxSubscriptions)
            return await Result<SubscriptionDto>.FailAsync("Max subscriptions reached");
        
        await _context.ChannelSubscriptions.AddAsync(new ChannelSubscription
        {
            GuildChannelId = command.ChannelId,
            SubscriptionId = subscription.Id,
        }, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return await _mediator.Send(new GetSubscriptionByIdQuery(subscription.Id));
    }

    public async Task<Result<Guid>> Handle(UnSubscribeCommand command, CancellationToken cancellationToken)
    {
        var subscription = await _context.Subscriptions.Include(x => x.ChannelSubscriptions).FirstOrDefaultAsync(subscription => subscription.SubredditId == command.SubredditId && subscription.Sort == command.Sort, cancellationToken);

        if (subscription == null)
            return (await Result.FailAsync("Not subscribed!")) as Result<Guid>;

        var channel = subscription.ChannelSubscriptions.FirstOrDefault(subscription => subscription.GuildChannelId == command.ChannelId);
        if (channel == null)
            return (await Result.FailAsync("Not subscribed!")) as Result<Guid>;

        subscription.ChannelSubscriptions.Remove(channel);
        
        if(subscription.ChannelSubscriptions.Any())
            _context.Subscriptions.Update(subscription);
        else
            _context.Subscriptions.Remove(subscription);
        

        await _context.SaveChangesAsync(cancellationToken);
        return await Result<Guid>.SuccessAsync(subscription.Id);
    }

    public async Task<Result> Handle(UpdatePostIdCommand command, CancellationToken cancellationToken)
    {
        var subscription = await _context.Subscriptions.FirstOrDefaultAsync(sub => sub.Id == command.SubscriptionId, cancellationToken);

        if (subscription == null)
            return (await Result.FailAsync("Subscription not found")) as Result;

        subscription.LastPostId = command.LastPostId;
        _context.Subscriptions.Update(subscription);
        await _context.SaveChangesAsync(cancellationToken);
        return await Result.SuccessAsync() as Result;
    }

    #endregion
}