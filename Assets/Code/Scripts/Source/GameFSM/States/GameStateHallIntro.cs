using System;
using Code.Scripts.Source.Audio;
using Code.Scripts.Source.Managers;
using Code.Scripts.Source.Narrator;
using UnityEngine;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateHallIntro : GameBaseState
    {
        [SerializeField] private VoiceLineSO _voiceLineSO;
        
        public override void EnterState(GameStateManager context)
        {
            AudioManager.Instance.ChangeRain(AudioManager.Instance.ClipsIndex.RainInDoor);
            AudioManager.Instance.ChangeMusic(AudioManager.Instance.ClipsIndex.GameMusic);
            context.SwitchState(context.GameStates.HallInProgress);
            Narrator.Narrator.Instance.PlayVoiceLine(_voiceLineSO);
        }

        public override void UpdateState(GameStateManager context)
        {

        }

        public override void ExitState(GameStateManager context)
        {

        }
    }
}
