namespace VrPlayer.Models.Settings
{
    public interface ISettingsManager
    {
        void Save();
        void Load();
    }
}