using UnityEngine;

namespace Code.Scripts.Source.Audio
{
    [RequireComponent(typeof(AudioSource), typeof(HingeJoint))]
    public class DoorSound : MonoBehaviour
    {
        [SerializeField] private float openAngleThreshold = 5f;
        [SerializeField] private float deadZone = 1f;
        [SerializeField] private bool opensPositively = true;

        private AudioSource _audioSource;
        private AudioClip _openSound;
        private AudioClip _closeSound;
        private HingeJoint _hinge;
        private bool _isOpen = false;

        private void Start()
        {
            _hinge = GetComponent<HingeJoint>();
            _audioSource = GetComponent<AudioSource>();
            _openSound = AudioManager.Instance.ClipsIndex.OpenDoor1;
            _closeSound = AudioManager.Instance.ClipsIndex.CloseDoor1;
            _audioSource.outputAudioMixerGroup = AudioManager.Instance.SFXMixerModule;
        }

        private void Update()
        {
            float angle = _hinge.angle;

            if (opensPositively)
            {
                if (!_isOpen && angle > openAngleThreshold + deadZone)
                {
                    _isOpen = true;
                    _audioSource.PlayOneShot(_openSound);
                }
                else if (_isOpen && angle < openAngleThreshold - deadZone)
                {
                    _isOpen = false;
                    _audioSource.PlayOneShot(_closeSound);
                }
            }
            else
            {
                if (!_isOpen && angle < -openAngleThreshold - deadZone)
                {
                    _isOpen = true;
                    _audioSource.PlayOneShot(_openSound);
                }
                else if (_isOpen && angle > -openAngleThreshold + deadZone)
                {
                    _isOpen = false;
                    _audioSource.PlayOneShot(_closeSound);
                }
            }
        }
    }
}