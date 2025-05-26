using Code.Scripts.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace Code.Scripts.Source.Audio
{
    public class AudioManager : MonoBehaviourSingleton<AudioManager>
    {
        [Space]
        [SerializeField] private AudioMixer _mixerSystem;

        [Space]
        public AudioClipsIndex ClipsIndex;

        [Header("Initial Mix")]
        [SerializeField] [Range(0, 1)] private float _initialMasterVolume = 0.5f;
        [SerializeField] [Range(0, 1)] private float _initialAmbienVolume = 1f;
        [SerializeField] [Range(0, 1)] private float _initialSFXVolume = 1f;

        [Header("Mixer Groups")]
        [field:SerializeField] public AudioMixerGroup MasterMixerModule;
        [field:SerializeField] public AudioMixerGroup AmbientMixerModule;
        [field:SerializeField] public AudioMixerGroup SFXMixerModule;

        [Header("Ambient Audio Sources")]
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private AudioSource _rainAudioSource;

        private static readonly string MasterVolumeParameter = "VolumeMaster";
        private static readonly string AmbientVolumeParameter = "VolumeAmbient";
        private static readonly string SFXVolumeParameter = "VolumeSFX";
        private static readonly string LowpassParameter = "LowPassFreqMaster";

        private float _masterVolume;
        public float MasterVolume {
            get => _masterVolume;

            set {
                _masterVolume = value;
                UpdateVolume(MasterVolumeParameter, value);
            }
        }

        private float _ambientVolume;
        public float AmbientVolume {
            get => _ambientVolume;

            set {
                _ambientVolume = value;
                UpdateVolume(AmbientVolumeParameter, value);
            }
        }

        private float _sfxVolume;
        public float SFXVolume {
            get => _sfxVolume;

            set {
                _sfxVolume = value;
                UpdateVolume(SFXVolumeParameter, value);
            }
        }

        private bool _masterVolumeMuted;
        private bool _ambientVolumeMuted;
        private bool _sfxVolumeMuted;
        public bool MasterVolumeMuted {
            get => _masterVolumeMuted;
            set {
                if (value)
                    UpdateVolume(MasterVolumeParameter, 0);
                else
                    UpdateVolume(MasterVolumeParameter, MasterVolume);
                _masterVolumeMuted = value;
            }
        }
        public bool AmbientVolumeMuted {
            get => _ambientVolumeMuted;
            set {
                if (value)
                    UpdateVolume(AmbientVolumeParameter, 0);
                else
                    UpdateVolume(AmbientVolumeParameter, AmbientVolume);
                _ambientVolumeMuted = value;
            }
        }
        public bool SFXVolumeMuted {
            get => _sfxVolumeMuted;
            set {
                if (value)
                    UpdateVolume(SFXVolumeParameter, 0);
                else
                    UpdateVolume(SFXVolumeParameter, SFXVolume);
                _sfxVolumeMuted = value;
            }
        }

        private void Awake()
        {
            MasterVolume = _initialMasterVolume;
            AmbientVolume = _initialAmbienVolume;
            SFXVolume = _initialSFXVolume;
        }

        /// <summary>
        /// Update the appropriate volume referenced as an exposed mixer parameter
        /// </summary>
        /// <remarks>🛈 The function is called upon setting the Volume properties and shouldn't be used as-is</remarks>
        /// <param name="mixerParameter">Name of the exposed mixer parameter to modify.</param>
        /// <param name="value">Normalized volume (automatically clamped between [0,1])</param>
        private void UpdateVolume(string mixerParameter, float value)
        {
            float decibels = -80 * (1 - Mathf.Clamp(value, 0, 1));

            _mixerSystem.SetFloat(mixerParameter, decibels);
        }

        /// <summary>
        /// Update manually the appropriate volume referenced as an exposed mixer parameter
        /// </summary>
        /// <remarks>⚠ Use with caution as volume value can exceed 1</remarks>
        /// <param name="mixerParameter">Name of the exposed mixer parameter to modify</param>
        /// <param name="value">Unclamped normalized volume</param>
        public void UpdateVolumeUnclamped(string mixerParameter, float value)
        {
            float decibels = -80 * (1 - value);

            _mixerSystem.SetFloat(mixerParameter, decibels);
        }

        /// <summary>
        /// Set the low-pass filter frequency.
        /// </summary>
        public void SetLowpassFrequency(float frequency)
        {
            _mixerSystem.SetFloat(LowpassParameter, frequency);
        }

        /// <summary>
        /// Change audio clip of the main music.
        /// </summary>
        public void ChangeMusic(AudioClip musicClip)
        {
            _musicAudioSource.clip = musicClip;
            _musicAudioSource.Play();
        }

        /// <summary>
        /// Change audio clip of rain ambient sound.
        /// </summary>
        public void ChangeRain(AudioClip rainClip)
        {
           _rainAudioSource.clip = rainClip;
           _rainAudioSource.Play();
        }
    }
}
