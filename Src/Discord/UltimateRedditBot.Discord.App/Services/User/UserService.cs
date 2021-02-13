using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UltimateRedditBot.Discord.Database;
using UltimateRedditBot.Discord.Domain.Dtos;
using UltimateRedditBot.Discord.Domain.Models;
using UltimateRedditBot.Infra.BaseRepository;

namespace UltimateRedditBot.Discord.App.Services.User
{
    public class UserService : IUserService
    {
        #region Constructor

        public UserService(IBaseRepository<Domain.Models.User, ulong, UltimateDiscordDbContext> userRepository,
            IBaseRepository<UserSettings, int, UltimateDiscordDbContext> userSettingsRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _userSettingsRepository = userSettingsRepository;
            _mapper = mapper;
        }

        #endregion

        #region Fields

        private readonly IBaseRepository<Domain.Models.User, ulong, UltimateDiscordDbContext> _userRepository;
        private readonly IBaseRepository<UserSettings, int, UltimateDiscordDbContext> _userSettingsRepository;
        private readonly IMapper _mapper;

        #endregion

        #region Methods

        public async Task<UserSettingsDto> GetUserSettingsById(ulong userId)
        {
            var userSettings = await _userSettingsRepository.GetAllAsync();
            var userSetting = userSettings.FirstOrDefault(x => x.UserId == userId);

            return _mapper.Map<UserSettingsDto>(userSetting);
        }

        public async Task SaveUserSettings(UserSettingsDto userSettingsDto)
        {
            //Ensure that the user exists.
            var user = await _userRepository.GetByIdAsync(userSettingsDto.UserId);
            if (user == null)
                await _userRepository.InsertAsync(new Domain.Models.User
                {
                    Id = userSettingsDto.UserId
                });

            var userSettings = _mapper.Map<UserSettings>(userSettingsDto);

            if (userSettings.Id != 0)
                await _userSettingsRepository.UpdateAsync(userSettings);
            else
                await _userSettingsRepository.InsertAsync(userSettings);
        }

        public async Task<UserDto> GetById(ulong userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return null;

            return _mapper.Map<UserDto>(user);
        }


        public Task RegisterUser(ulong userId)
        {
            return _userRepository.InsertAsync(new Domain.Models.User {Id = userId});
        }

        #endregion
    }
}
