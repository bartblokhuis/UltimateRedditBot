using System.Threading.Tasks;
using UltimateRedditBot.Domain.Models.Settings;

namespace UltimateRedditBot.Infra.Services
{
    public interface IGenericSettingService
    {
        Task<TObj> GetSettingValueByKeyGroupAndKey<TObj>(string keyGroup, string key);

        Task<GenericSetting> GetSettingByKeyGroupAndKey(string keyGroup, string key);

        Task SaveSetting(GenericSetting setting);
    }
}
