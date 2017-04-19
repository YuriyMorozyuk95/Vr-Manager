using System;
using System.Configuration;
using VrPlayer.Helpers;

namespace VrPlayer.Models.Config
{
    public class AppSettingsApplicationConfig : ApplicationConfigBase
    {
        public AppSettingsApplicationConfig()
        {
            try
            {
                DefaultMediaFile = ConfigurationManager.AppSettings["DefaultMediaFile"];
                CameraFieldOfView = int.Parse(ConfigurationManager.AppSettings["CameraFieldOfView"]);
                ViewportsHorizontalOffset = int.Parse(ConfigurationManager.AppSettings["ViewportsHorizontalOffset"]);
                ViewportsVerticalOffset = int.Parse(ConfigurationManager.AppSettings["ViewportsVerticalOffset"]);
                SamplesFolder = ConfigurationManager.AppSettings["SamplesFolder"];
                NeckHeight = ConfigHelper.ParseDouble(ConfigurationManager.AppSettings["NeckHeight"]);
                ReadSideCarPresets = bool.Parse(ConfigurationManager.AppSettings["ReadSideCarPresets"]);

                DefaultMedia = ConfigurationManager.AppSettings["DefaultMedia"];
                DefaultEffect = ConfigurationManager.AppSettings["DefaultEffect"];
                DefaultDistortion = ConfigurationManager.AppSettings["DefaultDistortion"];
                DefaultProjection = ConfigurationManager.AppSettings["DefaultProjection"];
                DefaultTracker = ConfigurationManager.AppSettings["DefaultTracker"];
                DefaultStabilizer = ConfigurationManager.AppSettings["DefaultStabilizer"];
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while loading application settings.", exc);
            }
        }
    }
}
