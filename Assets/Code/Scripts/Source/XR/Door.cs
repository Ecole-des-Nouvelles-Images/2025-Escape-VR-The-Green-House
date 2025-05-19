using System;
using System.Collections;
using Code.Scripts.Source.Managers;
using Code.Scripts.Source.Types;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using VRTemplateAssets.Scripts;

namespace Code.Scripts.Source.XR
{
    [RequireComponent(typeof(Animator))]
    public class Door : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] SceneType _destination;
        [SerializeField] private bool _isLocked;
        [SerializeField] GameObject _CloneKey;
        
        [Header("Animation")]
        [SerializeField] private string _triggerDoorAnimation;
        private Animator _doorAnimator;
        
        [Header("Sound")]
       // [SerializeField] AudioSource _doorSound;
      // [SerializeField] AudioSource _keySound;
        
        private XRKnob _knob;
        private XRSocketTagInteractor _keySocket;
   

        private void Awake()
        {
            _knob = GetComponentInChildren<XRKnob>();
            _keySocket = GetComponentInChildren<XRSocketTagInteractor?>();
            _doorAnimator = GetComponent<Animator>();
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
            if (!Mathf.Approximately(value, 0)) return;
            if (_isLocked) return;
            
            OpenDoor(_destination);
        }

        private void ResetHandle(SelectExitEventArgs selectExitEventArgs)
        {
            _knob.value = 1;
        }

        private void InsertKey(SelectEnterEventArgs selectEnterEventArgs)
        {
            Destroy(_keySocket.firstInteractableSelected.transform.gameObject);
            _CloneKey.SetActive(true);
            _isLocked = false;
            _keySocket.socketActive = false;
          //  _keySound?.Play();
        }

        private void OpenDoor(SceneType sceneType)
        {
            _doorAnimator.SetTrigger(_triggerDoorAnimation);
            SceneLoader.Instance.SwitchScene(sceneType);
        }
        
        

     
    }
}
