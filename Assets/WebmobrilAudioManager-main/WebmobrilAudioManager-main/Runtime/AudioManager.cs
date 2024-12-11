using UnityEngine;

namespace Webmobril.AudioManager
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        private AudioSource _soundAudio;
        private AudioSource _musicAudio;
        private AudioSource _voiceAudio;

        private void OnEnable()
        {
            AudioSettings.VocalSettingChanged += HandleVocalChanged;
            AudioSettings.MusicSettingChanged += HandleMusicChanged;
        }

        private void HandleMusicChanged(bool value)
        {
            if (value)
            {
                if (_musicAudio.clip == null) return;
                _musicAudio.Play();
            }
            else
            {
                _musicAudio.Stop();
            }
        }

        private void HandleVocalChanged(bool value)
        {
            if (value)
            {
                if (_voiceAudio.clip == null) return;
                _voiceAudio.Play();
            }
            else
            {
                _voiceAudio.Stop();
            }
        }

        private void Awake()
        {
            if (CheckInstance()) return;
            if (_soundAudio == null)
            {
                _soundAudio = gameObject.AddComponent<AudioSource>();
            }

            if (_musicAudio == null)
            {
                _musicAudio = gameObject.AddComponent<AudioSource>();
            }

            if (_voiceAudio == null)
            {
                _voiceAudio = gameObject.AddComponent<AudioSource>();
            }
        }

        private bool CheckInstance()
        {
            Instance = FindObjectOfType<AudioManager>();
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return true;
            }

            Instance = this;
            DontDestroyOnLoad(this);
            return false;
        }

        public void PlaySound(AudioClip audioClip, AudioOption audioOption = null)
        {
            AddAudioOptions(_soundAudio, audioOption);
            _soundAudio.clip = audioClip;
            if(!AudioSettings.GetAudioSound) return;
            _soundAudio.Play();
        }

        public void PlayMusic(AudioClip audioClip, AudioOption audioOption = null)
        {
            AddAudioOptions(_musicAudio, audioOption);
            _musicAudio.clip = audioClip;
            if(!AudioSettings.GetAudioMusic) return;
            _musicAudio.Play();
        }

        public void PlayVocal(AudioClip audioClip, AudioOption audioOption = null)
        {
            AddAudioOptions(_voiceAudio, audioOption);
            _voiceAudio.clip = audioClip;
            if(!AudioSettings.GetAudioVocal) return;
            _voiceAudio.Play();
        }

        private static void AddAudioOptions(AudioSource audioSource, AudioOption audioOption)
        {
            if (audioOption == null) return;
            audioSource.outputAudioMixerGroup = audioOption.AudioMixerGroup;
            audioSource.mute = audioOption.Mute;
            audioSource.bypassEffects = audioOption.ByPassEffects;
            audioSource.bypassListenerEffects = audioOption.ByPassListenerEffects;
            audioSource.bypassReverbZones = audioOption.ByPassReverbZone;
            audioSource.playOnAwake = audioOption.PlayOnAwake;
            audioSource.loop = audioOption.Loop;
        }

        public bool IsSoundPlaying()
        {
            return _soundAudio.isPlaying;
        }
        public bool IsMusicPlaying()
        {
            return _musicAudio.isPlaying;
        }
        public bool IsVocalPlaying()
        {
            return _voiceAudio.isPlaying;
        }

        public void StopSound()
        {
            _soundAudio.Stop();
        }

        public void StopMusic()
        {
            _musicAudio.Stop();
        }

        public void StopVocal()
        {
            _voiceAudio.Stop();
        }

        public void PauseSound()
        {
            _soundAudio.Pause();
        }

        public void PauseMusic()
        {
            _musicAudio.Pause();
        }

        public void PauseVocal()
        {
            _voiceAudio.Pause();
        }

        public void SoundVolume(float volume)
        {
            _soundAudio.volume = volume;
        }

        public void MusicVolume(float volume)
        {
            _musicAudio.volume = volume;
        }
        public void VocalVolume(float volume)
        {
            _voiceAudio.volume = volume;
        }
    }
}