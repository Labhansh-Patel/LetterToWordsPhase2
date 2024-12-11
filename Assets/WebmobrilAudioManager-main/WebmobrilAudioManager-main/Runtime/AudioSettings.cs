using System;

namespace Webmobril.AudioManager
{
    public static class AudioSettings
    {
        public static Action<bool> MusicSettingChanged;
        public static Action<bool> VocalSettingChanged;
        public static bool GetAudioSound { get; private set; } = true;
        public static bool GetAudioMusic { get; private set; } = true;
        public static bool GetAudioVocal { get; private set; } = true;
    
        public static void ToggleAudioSound(bool sound)
        {
            GetAudioSound = sound;
        } 
        public static void ToggleAudioMusic(bool music)
        {
            GetAudioMusic = music;
            MusicSettingChanged?.Invoke(music);
        } 
        public static void ToggleAudioVocal(bool vocal)
        {
            GetAudioVocal = vocal;
            VocalSettingChanged?.Invoke(vocal);
        } 
    }
}