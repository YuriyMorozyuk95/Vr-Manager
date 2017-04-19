using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrManager.Data.Abstract;

namespace VrManager.Data.Entity
{
    public class ModelSetting : BaseEntity
    {
        public AppTheme CurrentAppTheme { get; set; }
        public AccentTheme CurrentAppAccent { get; set; }
        public string PathToFolderFiles { get; set; }
        public string PathToLicense { get; set; }
        public DateTime? AllTimeOff { get; set; }
        public bool IsBackgroundImage { get; set; }
        public bool IsTransperentTile { get; set; }
        public bool IsKioskMode { get; set; }
        public int NumberRightMonitor { get; set; } = 1;
        public int NumberLeftMonitor { get; set; } = 0;
        public DateTime? TimeAdvertising { get; set; } 
    }
}
public enum AppTheme
{
    BaseDark,
    BaseLight
}

public enum AccentTheme
{
    Red,
    Green,
    Blue,
    Purple,
    Orange,
    Lime,
    Teal,
    Cyan,
    Indigo,
    Violet,
    Pink,
    Magenta,
    Crimson,
    Yellow,
    Brown,
    Olive,
    Sienna
}