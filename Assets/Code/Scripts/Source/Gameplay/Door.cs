using Code.Scripts.Source.Audio;
using Code.Scripts.Source.Managers;
using Code.Scripts.Source.Types;
using Code.Scripts.Source.XR;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using VRTemplateAssets.Scripts;

namespace Code.Scripts.Source.Gameplay
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class Door : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SceneType _destination;
        [SerializeField] private bool _isLocked;
        [FormerlySerializedAs("_animator")] [SerializeField] private Animator _ivyAnimator;
        [FormerlySerializedAs("_CloneKey")] [SerializeField] private GameObject _cloneKey;

        [Header("Animation")]
        [SerializeField] private string _triggerDoorAnimation;
        private Animator _doorAnimator;

        [Header("Sound")]
        private AudioSource _doorAudioSource;

        private XRKnob _knob;
        private XRSocketTagInteractor _keySocket;
        private bool _isOpen;

        private void Awake()
        {
            _knob = GetComponentInChildren<XRKnob>();
            _keySocket = GetComponentInChildren<XRSocketTagInteractor>();
            _doorAnimator = GetComponent<Animator>();
            _doorAudioSource = GetComponent<AudioSource>();

            if (!_keySocket && _isLocked)
                throw new System.Exception($"[Door] Key socket not found on locked door : {gameObject.name}");
        }


        private void OnEnable()
        {
            _knob.onValueChange.AddListener(DoorHandleUpdate);
            _knob.selectExited.AddListener(ResetHandle);
            _keySocket?.selectEntered.AddListener(InsertKey);
        }

        private void OnDisable()
        {
            _knob.onValueChange.RemoveListener(DoorHandleUpdate);
            _knob.selectExited.RemoveListener(ResetHandle);
            _keySocket?.selectEntered.RemoveListener(InsertKey);
        }

        private void DoorHandleUpdate(float value)
        {
            if (!Mathf.Approximately(value, 0f)) return;
            if (_isLocked) return;
            if (!Mathf.Approximately(value, 0f)) return;


            if (_isOpen) return;

            OpenDoor(_destination);
        }

        private void ResetHandle(SelectExitEventArgs selectExitEventArgs)
        {
            _knob.value = 1;
        }

        private void InsertKey(SelectEnterEventArgs selectEnterEventArgs)
        {
            Destroy(_keySocket.firstInteractableSelected.transform.gameObject);
            _cloneKey.SetActive(true);
            _isLocked = false;
            _keySocket.socketActive = false;

            PlayDoorSound( AudioManager.Instance.ClipsIndex.InsertKey);
        }

        private void OpenDoor(SceneType sceneType)
        {
            PlayDoorSound(AudioManager.Instance.ClipsIndex.OpenDoor);
            _isOpen = true;
            _doorAnimator.SetTrigger(_triggerDoorAnimation);
            SceneLoader.Instance.SwitchScene(sceneType);
        }

        private void PlayDoorSound (AudioClip soundClip)
        {
            _doorAudioSource.clip = soundClip;
            _doorAudioSource.Play();
        }
    }
}
