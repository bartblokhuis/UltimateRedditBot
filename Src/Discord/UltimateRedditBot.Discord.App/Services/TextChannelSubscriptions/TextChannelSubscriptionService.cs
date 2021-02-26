using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UltimateRedditBot.Discord.Database;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Infra.BaseRepository;

namespace UltimateRedditBot.Discord.App.Services
{
    public class TextChannelSubscriptionService : ITextChannelSubscriptionService
    {
        #region Fields

        private readonly IBaseRepository<TextChannelSubscription, int, UltimateDiscordDbContext> _textChannelSubsRepo;

        #endregion

        #region Constructor

        public TextChannelSubscriptionService(IBaseRepository<TextChannelSubscription, int, UltimateDiscordDbContext> textChannelSubsRepo)
        {
            _textChannelSubsRepo = textChannelSubsRepo;
        }

        #endregion

        #region Methods

        public Task<bool> IsSubscribed(int textChannelId, int subscriptionId)
        {
            return _textChannelSubsRepo.Table.AsQueryable().AnyAsync(x =>
                x.TextChannelId == textChannelId && x.SubscriptionId == subscriptionId);
        }

        public Task Subscribe(int textChannelId, int subscriptionId)
        {
            return _textChannelSubsRepo.InsertAsync(new TextChannelSubscription
            {
                SubscriptionId = subscriptionId,
                TextChannelId = textChannelId
            });
        }

        public async Task Unsubscribe(TextChannelSubscription textChannelSubscription)
        {
            if (textChannelSubscription != null)
                await _textChannelSubsRepo.DeleteAsync(textChannelSubscription);
        }

        public Task<List<TextChannelSubscription>> GetTextChannelSubscriptions()
        {
            return _textChannelSubsRepo.Table.Include(x => x.TextChannel).ToListAsync();
        }

        public Task<TextChannelSubscription> GetTextChannelSubscription(int textChannelId, int subscriptionId)
        {
            return _textChannelSubsRepo.Table.FirstOrDefaultAsync(x =>
                x.TextChannelId == textChannelId && x.SubscriptionId == subscriptionId);
        }

        #endregion
    }
}
