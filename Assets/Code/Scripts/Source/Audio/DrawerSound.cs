using UnityEngine;

namespace Code.Scripts.Source.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class DrawerMoveSoundSimple : MonoBehaviour
    {
        [SerializeField] private DrawerType _drawerType;
        [SerializeField] private float _soundDelay = 2f;
        [SerializeField] private float _movementThreshold = 0.05f;
        [SerializeField] private bool _useXAxis;

        private AudioSource _audioSource;
        private AudioClip _moveSound;
        private float _lastPos;
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
            _lastPos = GetAxisPosition();

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
            float currentPos = GetAxisPosition();
            float movement = Mathf.Abs(currentPos - _lastPos);

            if (movement > _movementThreshold && Time.time >= _nextSoundTime)
            {
                _audioSource.PlayOneShot(_moveSound);
                _nextSoundTime = Time.time + _soundDelay;
            }

            _lastPos = currentPos;
        }

        private float GetAxisPosition()
        {
            return _useXAxis ? transform.localPosition.x : transform.localPosition.z;
        }
    }
}
