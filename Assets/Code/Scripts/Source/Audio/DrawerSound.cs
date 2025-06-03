using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scripts.Source.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class DrawerMoveSoundSimple : MonoBehaviour
    {
        [SerializeField] private DrawerType _drawerType;
        [SerializeField] private float _soundDelay = 2f;
        [SerializeField] private float _movementThreshold = 0.001f;

        private AudioSource _audioSource;
        private AudioClip _moveSound;
        private float _lastZ;
        private float _nextSoundTime;

        private enum DrawerType
        {
            SmallDrawer,
            MediumDrawer1,
            MediumDrawer2,
            LargeDrawer
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _lastZ = transform.localPosition.z;

            switch (_drawerType)
            {
                case DrawerType.SmallDrawer:
                    _moveSound = AudioManager.Instance.ClipsIndex.OpenDrawer2;
                    break;
                case DrawerType.MediumDrawer1:
                    _moveSound = AudioManager.Instance.ClipsIndex.OpenDrawer1;
                    break;
                case DrawerType.MediumDrawer2:
                    _moveSound = AudioManager.Instance.ClipsIndex.OpenDrawer3;
                    break;
                case DrawerType.LargeDrawer:
                    _moveSound = AudioManager.Instance.ClipsIndex.OpenDrawer4;
                    break;
            }

            _nextSoundTime = Time.time;
        }

        private void Update()
        {
            float currentZ = transform.localPosition.z;
            float movement = Mathf.Abs(currentZ - _lastZ);

            if (movement > _movementThreshold && Time.time >= _nextSoundTime)
            {
                _audioSource.PlayOneShot(_moveSound);
                _nextSoundTime = Time.time + _soundDelay;
            }

            _lastZ = currentZ;
        }
    }
}