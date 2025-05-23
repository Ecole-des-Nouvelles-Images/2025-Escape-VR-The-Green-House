using System;
using Code.Scripts.Source.Managers;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateLoungeIntro : GameBaseState
    {
        public override void EnterState(GameStateManager context)
        {
            base.EnterState(context);

            context.SwitchState(context.GameStates.LoungePhase2);
        }

        public override void UpdateState(GameStateManager context)
        {

        }

        public override void ExitState(GameStateManager context)
        {

        }
    }
}
