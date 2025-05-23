using System;
using System.Collections.Generic;
using Code.Scripts.Source.Managers;
using Code.Scripts.Source.Types;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateMainMenu : GameBaseState
    {
        List<NearFarInteractor> _xrNearFarInteractors;

        public override void EnterState(GameStateManager context)
        {
            context.ChangeNearFarInteractionMode(NearFarMode.Far);

            Debug.Log("[GameStateMainMenu] MainMenu successfully loaded.");
        }

        public override void UpdateState(GameStateManager context)
        {
        }

        public override void ExitState(GameStateManager context)
        {
            context.ChangeNearFarInteractionMode(NearFarMode.Near);
        }
    }
}
