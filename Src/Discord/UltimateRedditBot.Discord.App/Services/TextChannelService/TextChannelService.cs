using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UltimateRedditBot.Discord.Database;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Infra.BaseRepository;

namespace UltimateRedditBot.Discord.App.Services.TextChannelService
{
    public class TextChannelService : ITextChannelService
    {
        #region Fields

        private readonly IBaseRepository<TextChannel, int, UltimateDiscordDbContext> _textChannelBaseRepo;

        #endregion

        #region Constructor

        public TextChannelService(IBaseRepository<TextChannel, int, UltimateDiscordDbContext> textChannelBaseRepo)
        {
            _textChannelBaseRepo = textChannelBaseRepo;
        }

        #endregion

        #region Methods

        public Task<TextChannel> GetTextChannelById(ulong id, ulong? guildId, ulong? userId)
        {
            return _textChannelBaseRepo.Table.AsQueryable()
                .FirstOrDefaultAsync(x => x.TextChannelId == id && x.GuildId == guildId && x.UserId == userId);
        }

        public async Task<TextChannel> RegisterTextChannel(ulong id, ulong? guildId, ulong? userId)
        {
            var textChannel = new TextChannel
            {
                TextChannelId = id,
                UserId = userId,
                GuildId = guildId
            };

            await _textChannelBaseRepo.InsertAsync(textChannel);
            return textChannel;
        }

        #endregion
    }
}
