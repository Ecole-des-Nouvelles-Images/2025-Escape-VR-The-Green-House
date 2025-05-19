using System;
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
        public static Action OnSocketChanged;
        public static Action OnFusePlugged;
        [SerializeField] private List<XRSocketInteractor> _bookSockets = new List<XRSocketInteractor>(5);
        [SerializeField] private List<string> _correctBookPlacement = new List<string>(5);
        private  bool _fusePlugged = false;
        private bool _puzzleSolved = false;
        private GameStateManager _ctx;
        private Action<GameBaseState, bool, bool> OnPuzzleSolved;


        public override void EnterState(GameStateManager context)
        {
            base.EnterState(context);

            _ctx = context;
            OnPuzzleSolved += context.SwitchState;
            OnSocketChanged += CheckPuzzle;
            OnFusePlugged += PlugFuseCheck;
        }

        public override void UpdateState(GameStateManager context)
        {

        }

        public override void ExitState(GameStateManager context)
        {
            OnPuzzleSolved -= context.SwitchState;
            OnSocketChanged -= CheckPuzzle;
            OnFusePlugged -= PlugFuseCheck;
        }


        private void CheckPuzzle()
        {
            Debug.Log("Checking Puzzle");
            if (_puzzleSolved || !_fusePlugged) return;

            for (int i = 0; i < _bookSockets.Count; i++)
            {
                if (!_bookSockets[i].hasSelection) return;

                GameObject selected = _bookSockets[i].GetOldestInteractableSelected().transform.gameObject;
                Book book = selected.GetComponent<Book>();

                if (book == null || book.BookName != _correctBookPlacement[i]) return;
            }

            _puzzleSolved = true;
            OnPuzzleSolved.Invoke(_ctx.GameStates.LaboratoryPhase1, false, false);
            Debug.Log(" Puzzle terminé");
        }

        private void PlugFuseCheck()
        {
            _fusePlugged = true;
        }


    }
}
