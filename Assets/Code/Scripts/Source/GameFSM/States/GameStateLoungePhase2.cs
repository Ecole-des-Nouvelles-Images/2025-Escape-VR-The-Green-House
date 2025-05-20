using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

using Code.Scripts.Source.Gameplay.Lounge;
using Code.Scripts.Source.Managers;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateLoungePhase2 : GameBaseState
    {
        [SerializeField] private List<string> _correctBookPlacement = new(5);

        public Action OnSocketChanged;
        public Action OnFusePlugged;

        private Action<GameBaseState, bool, bool> _onPuzzleSolved;

        private GameStateManager _ctx;
        private Transform _bookSocketsContainer;
        private List<XRSocketInteractor> _bookSockets = new(5);

        private bool _initialized;
        private bool _fusePlugged;
        private bool _puzzleSolved;

        public override void EnterState(GameStateManager context)
        {
            base.EnterState(context);

            if (!_initialized)
                Initialize(context);

            _ctx = context;

            _onPuzzleSolved += context.SwitchState;
            OnSocketChanged += CheckPuzzle;
            OnFusePlugged += PlugFuseCheck;
        }

        public override void UpdateState(GameStateManager context)
        {

        }

        public override void ExitState(GameStateManager context)
        {
            _onPuzzleSolved -= context.SwitchState;
            OnSocketChanged -= CheckPuzzle;
            OnFusePlugged -= PlugFuseCheck;
        }

        // ---

        private void Initialize(GameStateManager ctx)
        {
            ctx.StartCoroutine(GetXRSocketInteractors());
            _initialized = true;
        }

        private IEnumerator GetXRSocketInteractors()
        {
            while (SceneLoader.Instance.ActiveScene.name != "Lounge")
            {
                yield return null;
            }

            if (!_bookSocketsContainer)
                _bookSocketsContainer = GameObject.FindGameObjectWithTag("LoungeBookSocketsContainer").transform;

            if (_bookSockets.Count == 0)
            {
                foreach (Transform child in _bookSocketsContainer)
                    _bookSockets.Add(child.GetComponent<XRSocketInteractor>());
            }

            Debug.Log("[GameStateLoungePhase2] Sockets founded for Lounge");
        }

        private void CheckPuzzle()
        {
            Debug.Log("[GameStateLoungePhase2] Checking puzzle...");
            if (_puzzleSolved || !_fusePlugged) return;

            for (int i = 0; i < _bookSockets.Count; i++)
            {
                if (!_bookSockets[i].hasSelection) return;

                GameObject selected = _bookSockets[i].GetOldestInteractableSelected().transform.gameObject;
                Book book = selected.GetComponent<Book>();

                if (!book || book.BookName != _correctBookPlacement[i]) return;
            }

            _puzzleSolved = true;
            _onPuzzleSolved.Invoke(_ctx.GameStates.LaboratoryPhase1, false, false);
            Debug.Log("[GameStateLoungePhase2] Puzzle solved!");
        }

        private void PlugFuseCheck()
        {
            _fusePlugged = true;
        }
    }
}
