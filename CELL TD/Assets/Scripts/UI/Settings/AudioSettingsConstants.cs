using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.UI.Settings
{
    /// <summary>
    /// Holds constants that store the names and default values of audio settings.
    /// </summary>
    public static class AudioSettingsConstants
    {
        public const string MasterVolumeKey = "Audio_Volume_Master";
        public const string MusicVolumeKey = "Audio_Volume_Music";
        public const string SfxVolumeKey = "Audio_Volume_SFX";

        // These default volume values should be in the range of 0-100;
        public const int Default_MasterVolume = 90;
        public const int Default_SfxVolume = 90;
        public const int Default_MusicVolume = 75;
    }
}
