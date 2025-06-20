using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Source.Gameplay.Lounge;
using Code.Scripts.Source.Managers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateLoungePhase2 : GameBaseState
    {
        private static readonly int Open = Animator.StringToHash("Open");

        public override GameStatesIndex StateIndex { get; protected set; } = GameStatesIndex.GameStateLoungePhase2;

        public bool FusePlugged { get; private set; }

        public Action OnFusePlugged;
        public Action OnSocketChanged;

        [SerializeField] private List<string> _correctBookPlacement = new(5);
        [SerializeField] private Transform _bookSocketsContainer;
        [SerializeField] private List<XRSocketInteractor> _bookSockets = new(5);
        [SerializeField] private Animator _libraryAnimator;
        [SerializeField] private AudioSource _libraryAudio;

        private GameStateManager _ctx;

        private Action<GameBaseState, bool, bool> _onPuzzleSolved;

        private bool _initialized;
        private bool _puzzleSolved;

        public override void EnterState(GameStateManager context)
        {
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
            while (SceneLoader.Instance.ActiveScene.name != "Lounge") yield return null;

            if (!_bookSocketsContainer)
            {
                _bookSocketsContainer = GameObject.FindGameObjectWithTag("LoungeBookSocketsContainer").transform;
                _libraryAnimator = _bookSocketsContainer.parent.GetComponent<Animator>();
                _libraryAudio = _libraryAnimator.GetComponent<AudioSource>();

                if (!_bookSocketsContainer)
                    throw new Exception("[GameStateLoungePhase2] Book sockets container not found.");
            }

            if (_bookSockets.Count == 0)
                foreach (Transform child in _bookSocketsContainer)
                    _bookSockets.Add(child.GetComponent<XRSocketInteractor>());
        }

        private void CheckPuzzle()
        {
            if (_puzzleSolved || !FusePlugged) return;

            for (int i = 0; i < _bookSockets.Count; i++)
            {
                if (!_bookSockets[i].hasSelection) return;

                GameObject selected = _bookSockets[i].GetOldestInteractableSelected().transform.gameObject;
                Book book = selected.GetComponent<Book>();

                if (!book || book.BookName != _correctBookPlacement[i]) return;
            }

            _puzzleSolved = true;
            _onPuzzleSolved.Invoke(_ctx.GameStates.LaboratoryPhase1, false, false);
            _libraryAnimator.SetTrigger(Open);
            _libraryAudio.Play();
        }

        private void PlugFuseCheck()
        {
            FusePlugged = true;
        }
    }
}
