using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UltimateRedditBot.Discord.Database;
using UltimateRedditBot.Discord.Domain.Dtos;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Infra.BaseRepository;

namespace UltimateRedditBot.Discord.App.Services.Guild
{
    public class GuildService : IGuildService
    {
        #region Fields

        private readonly IBaseRepository<Guild, ulong, UltimateDiscordDbContext> _guildRepository;
        private readonly IBaseRepository<GuildSettings, int, UltimateDiscordDbContext> _guildSettingsRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public GuildService(IMapper mapper, IBaseRepository<Guild, ulong, UltimateDiscordDbContext> guildRepository, IBaseRepository<GuildSettings, int, UltimateDiscordDbContext> guildSettingsRepository)
        {
            _guildRepository = guildRepository;
            _guildSettingsRepository = guildSettingsRepository;
            _mapper = mapper;
        }

        #endregion

        public async Task InsertGuild(GuildDto guildDto)
        {
            var guild = _mapper.Map<Guild>(guildDto);
            await _guildRepository.InsertAsync(guild);
        }

        public async Task RegisterNewGuilds(IEnumerable<GuildDto> guilds)
        {
            var allGuilds = await _guildRepository.GetAllAsync();
            var newGuilds = guilds.Where(dto => allGuilds.All(guild => dto.Id != guild.Id))
                .Select(dto => _mapper.Map<Guild>(dto)).ToList();

            await _guildRepository.InsertAsync(newGuilds);
        }

        public Task<Guild> GetById(ulong guildId)
        {
            return _guildRepository.GetByIdAsync(guildId);
        }

        public async Task<GuildSettingsDto> GetGuildSettingsById(ulong guildId)
        {
            //TODO Improve db call
            var settings = await _guildSettingsRepository.GetAllAsync();
            var guildSettings = settings.FirstOrDefault(setting => setting.GuildId == guildId);

            return guildSettings == null ? null : _mapper.Map<GuildSettingsDto>(guildSettings);
        }

        public async Task SaveGuildSettings(GuildSettingsDto guildSettingsDto)
        {
            var guildSettings = _mapper.Map<GuildSettings>(guildSettingsDto);

            if (guildSettings.Id != 0)
                await _guildSettingsRepository.UpdateAsync(guildSettings);
            else
                await _guildSettingsRepository.InsertAsync(guildSettings);
        }
    }
}
