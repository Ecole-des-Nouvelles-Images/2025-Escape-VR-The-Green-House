using System;
using Code.Scripts.Source.Managers;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateLaunch : GameBaseState
    {
        public override GameStatesIndex StateIndex { get; protected set; } = GameStatesIndex.GameStateLaunch;

        public override void EnterState(GameStateManager context)
        {
            context.SwitchState(context.GameStates.HallIntro);
        }

        public override void UpdateState(GameStateManager context)
        {

        }

        public override void ExitState(GameStateManager context)
        {

        }
    }
}
