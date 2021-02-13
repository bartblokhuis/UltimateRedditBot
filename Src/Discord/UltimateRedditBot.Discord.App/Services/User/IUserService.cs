using System.Threading.Tasks;
using UltimateRedditBot.Discord.Domain.Dtos;

namespace UltimateRedditBot.Discord.App.Services.User
{
    public interface IUserService
    {
        Task<UserSettingsDto> GetUserSettingsById(ulong userId);

        Task SaveUserSettings(UserSettingsDto userSettingsDto);

        Task<UserDto> GetById(ulong userId);

        Task RegisterUser(ulong userId);


    }
}
