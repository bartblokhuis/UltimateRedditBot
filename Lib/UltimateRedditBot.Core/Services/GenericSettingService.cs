using System;
using System.Threading.Tasks;
using UltimateRedditBot.Domain.Models.Settings;
using UltimateRedditBot.Infra.BaseRepository;
using UltimateRedditBot.Infra.Services;
using Microsoft.EntityFrameworkCore;

namespace UltimateRedditBot.Core.Services
{
    public class GenericSettingService : IGenericSettingService
    {
        #region Fields

        private readonly IBaseRepository<GenericSetting> _genericSettingRepo;

        #endregion

        #region Constructor

        public GenericSettingService(IBaseRepository<GenericSetting> genericSettingRepo)
        {
            _genericSettingRepo = genericSettingRepo;
        }

        #endregion

        public async Task<TObj> GetSettingValueByKeyGroupAndKey<TObj>(string keyGroup, string key)
        {
            var setting = await GetSettingByKeyGroupAndKey(keyGroup, key);

            if (typeof(TObj).BaseType == typeof(Enum))
            {
                return (TObj)Enum.Parse(typeof(TObj), setting.Value, true);
            }

            return (TObj)Convert.ChangeType(setting.Value, typeof(TObj));
        }

        public Task<GenericSetting> GetSettingByKeyGroupAndKey(string keyGroup, string key)
        {
            var setting = _genericSettingRepo.Table.FirstOrDefaultAsync(genericSetting =>
                genericSetting.KeyGroup.Equals(keyGroup) && genericSetting.Key.Equals(key));

            return setting;
        }

        public Task SaveSetting(GenericSetting setting)
        {
            return _genericSettingRepo.InsertAsync(setting);
        }
    }
}
