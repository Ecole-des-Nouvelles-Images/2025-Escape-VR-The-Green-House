using System;
using Code.Scripts.Source.Managers;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateLoungeIntro : GameBaseState
    {
        public override GameStatesIndex StateIndex { get; protected set; } = GameStatesIndex.GameStateLoungeIntro;

        public override void EnterState(GameStateManager context)
        {
            context.SwitchState(context.GameStates.LoungePhase1); // phase 1 dans version finale
        }

        public override void UpdateState(GameStateManager context)
        {

        }

        public override void ExitState(GameStateManager context)
        {

        }
    }
}
