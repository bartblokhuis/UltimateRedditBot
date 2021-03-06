using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UltimateRedditBot.Discord.Database;
using UltimateRedditBot.Discord.Domain.Dtos;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Infra.BaseRepository;

namespace UltimateRedditBot.Discord.App.Services.Guild
{
    public class GuildService : IGuildService
    {
        #region Constructor

        public GuildService(IMapper mapper,
            IBaseRepository<Domain.Models.Guild, ulong, UltimateDiscordDbContext> guildRepository,
            IBaseRepository<GuildSettings, int, UltimateDiscordDbContext> guildSettingsRepository)
        {
            _guildRepository = guildRepository;
            _guildSettingsRepository = guildSettingsRepository;
            _mapper = mapper;
        }

        #endregion

        public async Task InsertGuild(GuildDto guildDto)
        {
            var guild = _mapper.Map<Domain.Models.Guild>(guildDto);
            await _guildRepository.InsertAsync(guild);
        }

        public async Task RegisterNewGuilds(IEnumerable<GuildDto> guilds)
        {
            var allGuilds = await _guildRepository.GetAllAsync();
            var newGuilds = guilds.Where(dto => allGuilds.All(guild => dto.Id != guild.Id))
                .Select(dto => _mapper.Map<Domain.Models.Guild>(dto)).ToList();

            await _guildRepository.InsertAsync(newGuilds);
        }

        public Task<Domain.Models.Guild> GetById(ulong guildId)
        {
            return _guildRepository.GetByIdAsync(guildId);
        }

        public string GetPrefix(ulong guildId)
        {
            return _guildSettingsRepository.Table.AsNoTracking().FirstOrDefault(x => x.GuildId == guildId)?.Prefix;
        }

        public GuildSettings GetGuildSettingsById(ulong guildId)
        {
            var guildSetting = _guildSettingsRepository.Table.AsTracking().FirstOrDefault(setting => setting.GuildId == guildId);
            return guildSetting;
        }

        public async Task SaveGuildSettings(GuildSettings guildSettings)
        {
            if (guildSettings.Id != 0)
                await _guildSettingsRepository.SaveChanges();
            else
                await _guildSettingsRepository.InsertAsync(guildSettings);
        }

        #region Fields

        private readonly IBaseRepository<Domain.Models.Guild, ulong, UltimateDiscordDbContext> _guildRepository;
        private readonly IBaseRepository<GuildSettings, int, UltimateDiscordDbContext> _guildSettingsRepository;
        private readonly IMapper _mapper;

        #endregion
    }
}
