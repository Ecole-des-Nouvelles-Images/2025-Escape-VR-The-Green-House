using Code.Scripts.Source.Audio;
using UnityEngine;

namespace Code.Scripts.Source.Gameplay.GreenHouse
{
    [RequireComponent(typeof(AudioSource))]
    public class OnTilt : MonoBehaviour
    {
        private enum ToolType
        {
            WateringCan,
            SeedBag
        }
    
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private ToolType _toolType;
        [Range(0,180)] [SerializeField] private float _orientationTreshold = 40f;
        private float angle;
        private AudioSource _audioSource;
        private bool _sfxIsPlaying;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.outputAudioMixerGroup = AudioManager.Instance.SFXMixerModule;
            _audioSource.playOnAwake = false;
            _audioSource.loop = true;
        }

        private void Update()
        {
            CheckOrientation();
        }

        private void CheckOrientation()
        {
            switch (_toolType)
            {
                case ToolType.SeedBag:
                    angle = Vector3.Angle(transform.up, Vector3.down);
                    break;
                case ToolType.WateringCan:
                    angle = Vector3.Angle(transform.forward, Vector3.down);
                    break;
            }

            if (angle < _orientationTreshold)
            {
                OnWaterBegin();
            }
            else
            {
                OnWaterEnd();
            }
        }
    
        private void OnWaterBegin()
        {
            if (!_particle.isPlaying) _particle.Play();

            if (_sfxIsPlaying) return;
            switch (_toolType)
            {
                case ToolType.SeedBag:
                    _audioSource.clip = AudioManager.Instance.ClipsIndex.Seed;
                    break;
                case ToolType.WateringCan:
                    _audioSource.clip = AudioManager.Instance.ClipsIndex.WaterCan;
                    break;
            }
            _audioSource.Play();
            _sfxIsPlaying = true;


        }
    
        private void OnWaterEnd()
        {
            if (_particle.isPlaying) _particle.Stop();
            if (!_sfxIsPlaying) return;
            _audioSource.Stop();
            _sfxIsPlaying = false;

        }
    
    }
}
