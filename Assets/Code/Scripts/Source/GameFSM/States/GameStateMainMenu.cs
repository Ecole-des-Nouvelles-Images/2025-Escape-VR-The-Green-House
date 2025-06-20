using System;
using System.Collections.Generic;
using Code.Scripts.Source.Audio;
using Code.Scripts.Source.Managers;
using Code.Scripts.Source.Types;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateMainMenu : GameBaseState
    {
        List<NearFarInteractor> _xrNearFarInteractors;

        public override GameStatesIndex StateIndex { get; protected set; } = GameStatesIndex.GameStateMainMenu;

        public override void EnterState(GameStateManager context)
        {
            context.ChangeNearFarInteractionMode(NearFarMode.Far);

           AudioManager.Instance.ChangeRain(AudioManager.Instance.ClipsIndex.RainOutDoor);
           AudioManager.Instance.ChangeMusic(AudioManager.Instance.ClipsIndex.MenuMusic);
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
