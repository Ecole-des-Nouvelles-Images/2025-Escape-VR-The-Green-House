using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Code.Scripts.Source.Audio
{
    [RequireComponent(typeof(AudioSource), typeof(XRGrabInteractable))]
    public class DrawerSound : MonoBehaviour
    {
        private enum DrawerType
        {
            SmallDrawer,
            MediumDrawer1,
            MediumDrawer2,
            LargeDrawer
        }

        [SerializeField] private DrawerType _drawerType;
        [SerializeField] private float _thresholdOffset = -0.02f;

        private AudioClip _openSound;
        private AudioClip _closeSound;
        private AudioSource _audioSource;
        private XRGrabInteractable _grab;
        private float _closedPosition;
        private float _openThreshold;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _grab = GetComponent<XRGrabInteractable>();
            _grab.selectExited.AddListener(_ => OnReleased());
            _audioSource.outputAudioMixerGroup = AudioManager.Instance.SFXMixerModule;

            _openThreshold = _closedPosition + _thresholdOffset;

            switch (_drawerType)
            {
                case DrawerType.SmallDrawer:
                    _openSound = AudioManager.Instance.ClipsIndex.OpenDrawer2;
                    _closeSound = AudioManager.Instance.ClipsIndex.CloseDrawer2;
                    break;
                case DrawerType.MediumDrawer1:
                    _openSound = AudioManager.Instance.ClipsIndex.OpenDrawer1;
                    _closeSound = AudioManager.Instance.ClipsIndex.CloseDrawer1;
                    break;
                case DrawerType.MediumDrawer2:
                    _openSound = AudioManager.Instance.ClipsIndex.OpenDrawer3;
                    _closeSound = AudioManager.Instance.ClipsIndex.CloseDrawer3;
                    break;
                case DrawerType.LargeDrawer:
                    _openSound = AudioManager.Instance.ClipsIndex.OpenDrawer4;
                    _closeSound = AudioManager.Instance.ClipsIndex.CloseDrawer4;
                    break;
            }
        }
        
             
        private void OnReleased()
        {
            float releasePosition = transform.localPosition.z;
            if (releasePosition <= _openThreshold)
            {
                _audioSource.PlayOneShot(_openSound);
            }
            else if (releasePosition >= _openThreshold)
            {
                _audioSource.PlayOneShot(_closeSound);
            }

        }
        
    }
}