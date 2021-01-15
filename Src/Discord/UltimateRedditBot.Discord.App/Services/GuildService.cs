using System.Threading.Tasks;
using AutoMapper;
using UltimateRedditBot.Discord.Database;
using UltimateRedditBot.Discord.Domain.Dtos;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Infra.BaseRepository;

namespace UltimateRedditBot.Discord.App.Services
{
    public class GuildService : IGuildService
    {
        #region Fields

        private readonly IBaseRepository<Guild, ulong, UltimateDiscordDbContext> _guildRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public GuildService(IMapper mapper, IBaseRepository<Guild, ulong, UltimateDiscordDbContext> guildRepository)
        {
            _guildRepository = guildRepository;
            _mapper = mapper;
        }

        #endregion

        public async Task AddGuild(GuildDto guildDto)
        {
            var guild = _mapper.Map<Guild>(guildDto);
            await _guildRepository.InsertAsync(guild);
        }
    }
}
