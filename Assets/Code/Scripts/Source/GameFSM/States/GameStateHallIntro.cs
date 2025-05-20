using System;
using Code.Scripts.Source.Managers;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateHallIntro : GameBaseState
    {
       
        public override void EnterState(GameStateManager context)
        {
            context.SwitchState(context.GameStates.HallInProgress);
        }

        public override void UpdateState(GameStateManager context)
        {

        }

        public override void ExitState(GameStateManager context)
        {

        }
    }
}
