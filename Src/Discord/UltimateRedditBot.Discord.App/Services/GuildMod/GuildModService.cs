using System.Threading.Tasks;
using UltimateRedditBot.Discord.App.Services.User;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Infra.BaseRepository;
using System.Linq;
using UltimateRedditBot.Discord.Database;

namespace UltimateRedditBot.Discord.App.Services
{
    public class GuildModService : IGuildModService
    {
        #region Fields

        private readonly IBaseRepository<GuildMod,int, UltimateDiscordDbContext> _guildModRepo;
        private readonly IUserService _userService;

        #endregion

        #region Constructor

        public GuildModService(IBaseRepository<GuildMod,int, UltimateDiscordDbContext> guildModRepo, IUserService userService)
        {
            _guildModRepo = guildModRepo;
            _userService = userService;
        }

        #endregion

        #region Methods

        public async Task<string> Mod(ulong userId, ulong guildId)
        {
            if (IsMod(userId, guildId))
                return "User is already a mod";

            var user = await _userService.GetById(userId);
            if (user == null)
                await _userService.RegisterUser(userId);

            await _guildModRepo.InsertAsync(new GuildMod
            {
                GuildId = guildId,
                UserId = userId
            });

            return string.Empty;
        }

        public bool IsMod(ulong userId, ulong guildId)
        {
            return _guildModRepo.Table.Any(x => x.UserId == userId && x.GuildId == guildId);
        }

        public async Task<string> UnMod(ulong userId, ulong guildId)
        {
            var mod = await _guildModRepo.Table.FirstOrDefaultAsync(x => x.UserId == userId && x.GuildId == guildId);
            if (mod == null)
                return "User isn't a mod";

            await _guildModRepo.DeleteAsync(mod);
            return string.Empty;
        }

        #endregion

    }
}
