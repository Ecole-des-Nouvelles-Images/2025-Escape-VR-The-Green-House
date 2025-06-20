using Code.Scripts.Source.Managers;
using Code.Scripts.Source.Narrator;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Code.Scripts.Source.Gameplay.Lounge
{
    public class BookSocket : MonoBehaviour
    {
        [SerializeField] private VoiceLineSO _fuseWantedVoiceLine;
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
                GameStateManager.Instance.GameStates.LoungePhase2.OnFusePlugged?.Invoke();

            else if (!GameStateManager.Instance.GameStates.LoungePhase2.FusePlugged)
                Narrator.Narrator.Instance.PlayVoiceLine(_fuseWantedVoiceLine);

            GameStateManager.Instance.GameStates.LoungePhase2.OnSocketChanged?.Invoke();
        }

        private void OnBookRemoved(SelectExitEventArgs args)
        {
            GameStateManager.Instance.GameStates.LoungePhase2.OnSocketChanged?.Invoke();
        }
    }
}
