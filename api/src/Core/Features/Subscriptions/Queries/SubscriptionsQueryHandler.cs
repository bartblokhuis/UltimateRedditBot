using AutoMapper;
using Core.Features.Common;
using Core.Features.Subscriptions.Queries;
using Database;
using Domain.Dtos;
using Domain.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Subscriptions.Commands;

internal class SubscriptionsQueryHandler : BaseQueryHandler, IRequestHandler<GetSubsciptionsQuery, ListResult<SubscriptionDto>>, IRequestHandler<GetSubscriptionByIdQuery, Result<SubscriptionDto>>, 
    IRequestHandler<GetSubscriptionsByGuildIdQuery, ListResult<SubscriptionDto>>
{
    #region Constructor

    public SubscriptionsQueryHandler(UltimateRedditBotDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    #endregion

    #region Methods

    public Task<ListResult<SubscriptionDto>> Handle(GetSubsciptionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Subscriptions
            .Include(x => x.ChannelSubscriptions).ThenInclude(x => x.GuildChannel).ThenInclude(x => x.Guild).ThenInclude(x => x.GuildSetting)
            .Include(x => x.Subreddit)
            .Select(q => new SubscriptionDto
            {
                Id = q.Id,
                LastPostId = q.LastPostId,
                Sort = q.Sort,
                SubredditName = q.Subreddit.Name,
                Channels = q.ChannelSubscriptions.Select(x => new ChannelSubscriptionDto
                {
                    Id = x.GuildChannelId,
                    GuildId = x.GuildChannel.Guild.Id,
                    DiscordChannelId = x.GuildChannel.DiscordChannelId,
                    IsEnabled = x.GuildChannel.Guild.GuildSetting.IsEnabled,
                    IsShowNsfw = x.GuildChannel.Guild.GuildSetting.AllowNsfw
                }).ToList()
            }).ToListResultAsync(cancellationToken);

        return query;
    }

    public async Task<Result<SubscriptionDto>> Handle(GetSubscriptionByIdQuery request, CancellationToken cancellationToken)
    {
        var subscription = await _context.Subscriptions
            .Include(x => x.ChannelSubscriptions).ThenInclude(x => x.GuildChannel).ThenInclude(x => x.Guild).ThenInclude(x => x.GuildSetting)
            .Include(x => x.Subreddit)
            .FirstOrDefaultAsync(subscription => subscription.Id == request.SubscriptionId, cancellationToken);

        if (subscription == null)
            return await Result<SubscriptionDto>.FailAsync("Subscription not found");

        var dto = new SubscriptionDto
        { 
            Id = subscription.Id,
            LastPostId = subscription.LastPostId,
            Sort = subscription.Sort,
            SubredditName = subscription.Subreddit.Name,
            Channels = subscription.ChannelSubscriptions.Select(x => new ChannelSubscriptionDto
            {
                Id = x.GuildChannelId,
                GuildId = x.GuildChannel.Guild.Id,
                DiscordChannelId = x.GuildChannel.DiscordChannelId,
                IsEnabled = x.GuildChannel.Guild.GuildSetting.IsEnabled,
                IsShowNsfw = x.GuildChannel.Guild.GuildSetting.AllowNsfw
            }).ToList()
        };

        return await Result<SubscriptionDto>.SuccessAsync(dto);
    }

    public Task<ListResult<SubscriptionDto>> Handle(GetSubscriptionsByGuildIdQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Subscriptions
            .Include(x => x.ChannelSubscriptions).ThenInclude(x => x.GuildChannel).ThenInclude(x => x.Guild).ThenInclude(x => x.GuildSetting)
            .Include(x => x.Subreddit)
            .Where(x => x.ChannelSubscriptions.Any(y => y.GuildChannel.GuildId == request.GuildId));

        if (!request.ShowNsfw)
            query = query.Where(x => !x.Subreddit.IsNsfw);

        return query.Select(q => new SubscriptionDto
        {
            Id = q.Id,
            LastPostId = q.LastPostId,
            Sort = q.Sort,
            SubredditName = q.Subreddit.Name,
            Channels = q.ChannelSubscriptions.Where(x => x.GuildChannel.Guild.Id == request.GuildId).Select(x => new ChannelSubscriptionDto
            {
                Id = x.GuildChannelId,
                GuildId = x.GuildChannel.Guild.Id,
                DiscordChannelId = x.GuildChannel.DiscordChannelId,
                IsEnabled = x.GuildChannel.Guild.GuildSetting.IsEnabled,
                IsShowNsfw = x.GuildChannel.Guild.GuildSetting.AllowNsfw
            }).ToList()
        }).ToListResultAsync(cancellationToken);
    }

    #endregion
}