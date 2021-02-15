using System.Threading.Tasks;
using UltimateRedditBot.Discord.Database;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Infra.BaseRepository;

namespace UltimateRedditBot.Discord.App.Services.TextChannelService
{
    public class TextChannelService : ITextChannelService
    {
        #region Fields

        private readonly IBaseRepository<TextChannel, ulong, UltimateDiscordDbContext> _textChannelBaseRepo;

        #endregion

        #region Constructor

        public TextChannelService(IBaseRepository<TextChannel, ulong, UltimateDiscordDbContext> textChannelBaseRepo)
        {
            _textChannelBaseRepo = textChannelBaseRepo;
        }

        #endregion

        #region Methods

        public Task<TextChannel> GetTextChannelById(ulong id)
        {
            return _textChannelBaseRepo.GetByIdAsync(id);
        }

        public Task RegisterTextChannel(ulong id, ulong? userId, ulong? guilId)
        {
            var textChannel = new TextChannel
            {
                Id = id,
                UserId = userId,
                GuildId = guilId
            };

            return _textChannelBaseRepo.InsertAsync(textChannel);
        }

        #endregion
    }
}
