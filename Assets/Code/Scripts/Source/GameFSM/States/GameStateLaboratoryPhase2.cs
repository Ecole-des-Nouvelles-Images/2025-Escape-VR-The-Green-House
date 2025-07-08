using System;
using Code.Scripts.Source.Managers;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateLaboratoryPhase2 : GameBaseState
    {
        public override GameStatesIndex StateIndex { get; protected set; } = GameStatesIndex.GameStateLaboratoryPhase2;

        public override void EnterState(GameStateManager context) {}

        public override void UpdateState(GameStateManager context)
        {

        }

        public override void ExitState(GameStateManager context)
        {

        }
    }
}
