using System;
using Code.Scripts.Source.Managers;
using UnityEngine;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateHallInProgress: GameBaseState
    {
        public static Action<GameBaseState, bool, bool> OnCodeFound;
        public static Action<string, int> OnRotated;

        [SerializeField] private int[] _correctCode;
        private int[] _currentCode;
        private GameStateManager _ctx;

        public override void EnterState(GameStateManager context)
        {
            base.EnterState(context);

            _ctx = context;
            OnRotated += CheckResults;
            OnCodeFound += context.SwitchState;
            _currentCode = new [] {0, 0, 0, 0};
        }

        public override void UpdateState(GameStateManager context)
        {

        }

        public override void ExitState(GameStateManager context)
        {
            
            OnRotated -= CheckResults;
            OnCodeFound -= context.SwitchState;
        }

        private void CheckResults(string wheelName, int wheelNumber)
        {
            switch (wheelName)
            {
                case "Wheel1":
                    _currentCode[0] = wheelNumber;
                    break;
                case "Wheel2":
                    _currentCode[1] = wheelNumber;
                    break;
                case "Wheel3":
                    _currentCode[2] = wheelNumber;
                    break;
                case "Wheel4":
                    _currentCode[3] = wheelNumber;
                    break;
            }

            if (_currentCode[0] == _correctCode[0] && _currentCode[1] == _correctCode[1] &&
                _currentCode[2] == _correctCode[2] && _currentCode[3] == _correctCode[3])
            {
                UnlockLock();
            }
        }

        private void UnlockLock()
        {
            OnCodeFound?.Invoke(_ctx.GameStates.HallResolved, false, false);
            //animation
            //disable padlock
        }
    }
}
