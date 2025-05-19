using System;
using Code.Scripts.Source.GameFSM.States;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Code.Scripts.Source.Gameplay.Lounge
{
    public class BookSocket : MonoBehaviour
    {
        private XRSocketInteractor _socket;

        private void Awake()
        {  
            _socket = GetComponent<XRSocketInteractor>();
        }
        
        private void OnEnable()
    {
        _socket.selectEntered.AddListener(OnBookPlaced);
        _socket.selectExited.AddListener(OnBookRemoved);
    }

    private void OnDisable()
    {
        _socket.selectEntered.RemoveListener(OnBookPlaced);
        _socket.selectExited.RemoveListener(OnBookRemoved);
    }

    private void OnBookPlaced(SelectEnterEventArgs args)
    {
        if (_socket.firstInteractableSelected.transform.CompareTag("Fuse"))
        {
            Debug.Log("Fuse placed");
            GameStateLoungePhase2.OnFusePlugged?.Invoke();
            _socket.socketActive = false;
        }
        GameStateLoungePhase2.OnSocketChanged?.Invoke();
    }

    private void OnBookRemoved(SelectExitEventArgs args)
    {
        GameStateLoungePhase2.OnSocketChanged?.Invoke();
    }
}
    
}
