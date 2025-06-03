using UnityEngine;

namespace Code.Scripts.Source.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class DrawerOpenCloseSound : MonoBehaviour
    {
        [SerializeField] private DrawerType _drawerType;
        [SerializeField] private  float _openThreshold = 0.1f;
        [SerializeField] private  float _deadZone = 0.005f;
        private  float _closedPosition;
        private AudioSource _audioSource;
        private AudioClip _closeSound;
        private bool _isOpen;

        private AudioClip _openSound;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            switch (_drawerType)
            {
                case DrawerType.SmallDrawer:
                    _openSound = AudioManager.Instance.ClipsIndex.OpenDrawer1;
                    _closeSound = AudioManager.Instance.ClipsIndex.CloseDrawer1;
                    break;
                case DrawerType.MediumDrawer1:
                    _openSound = AudioManager.Instance.ClipsIndex.OpenDrawer2;
                    _closeSound = AudioManager.Instance.ClipsIndex.CloseDrawer3;
                    break;
                case DrawerType.MediumDrawer2:
                    _openSound = AudioManager.Instance.ClipsIndex.OpenDrawer3;
                    _closeSound = AudioManager.Instance.ClipsIndex.CloseDrawer3;
                    break;
                case DrawerType.LargeDrawer:
                    _openSound = AudioManager.Instance.ClipsIndex.OpenDrawer4;
                    _closeSound = AudioManager.Instance.ClipsIndex.CloseDrawer4;
                    break;
                default: break;
            }

            _closedPosition = transform.localPosition.z;
        }

        void Update()
        {
            float currentZ = transform.localPosition.z;
            float distance = currentZ - _closedPosition;

            if (!_isOpen && distance > _openThreshold + _deadZone)
            {
                _isOpen = true;
                _audioSource.PlayOneShot(_openSound);
            }
            else if (_isOpen && distance < _openThreshold - _deadZone)
            {
                _isOpen = false;
                _audioSource.PlayOneShot(_closeSound);
            }
        }

        private enum DrawerType
        {
            SmallDrawer,
            MediumDrawer1,
            MediumDrawer2,
            LargeDrawer,
        }
    }
}