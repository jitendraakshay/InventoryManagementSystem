using DomainEntities;
using System.Collections.Generic;
namespace DomainInterface
{
    public interface ISettingsRepo
    {
        IEnumerable<Settings> GetSettings();

        Settings GetSettingsBySettingsId(string SettingsId);

        bool UpdateSettings(string SettingsId, string DefaultValue);

        int GetSessionTimeout();

        string GetSettingByIDandGroup(string SettingsID, string OfGroups);

    }
}
