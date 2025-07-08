using System;
using Code.Scripts.Source.Managers;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateLaboratoryIntro: GameBaseState
    {
        public override GameStatesIndex StateIndex { get; protected set; } = GameStatesIndex.GameStateLaboratoryIntro;

        public override void EnterState(GameStateManager context) {}

        public override void UpdateState(GameStateManager context)
        {

        }

        public override void ExitState(GameStateManager context)
        {

        }
    }
}
