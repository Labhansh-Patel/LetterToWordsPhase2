using UnityEngine.Audio;

namespace Webmobril.AudioManager
{
    public class AudioOption
    {
        public AudioMixerGroup AudioMixerGroup;
        public bool Mute { get; set; }
        public bool ByPassEffects { get; set; }
        public bool ByPassListenerEffects { get; set; }
        public bool ByPassReverbZone { get; set; }
        public bool PlayOnAwake { get; set; }
        public bool Loop { get; set; }
    }
}