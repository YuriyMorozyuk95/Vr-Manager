namespace VrPlayer.Models.Presets
{
    public interface IPresetsManager
    {
        void SaveMediaToFile(string fileName);
        void SaveDeviceToFile(string fileName);
        void SaveAllToFile(string fileName);
        void LoadFromUri(string path);
        bool LoadFromMetadata(string fileName);
    }
}