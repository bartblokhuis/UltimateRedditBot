using System.Threading.Tasks;
using UltimateRedditBot.Domain.Models.Settings;

namespace UltimateRedditBot.Infra.Services
{
    public interface IGenericSettingService
    {
        Task<TObj> GetSettingValueByKeyGroupAndKey<TObj>(string keyGroup, string key, string entityId);

        Task<GenericSetting> GetSettingByKeyGroupAndKey(string keyGroup, string key, string entityId);

        Task SaveSetting(GenericSetting setting);
    }
}
