using System;
using Code.Scripts.Source.Managers;
using Code.Scripts.Source.Narrator;
using UnityEngine;
using UnityEngine.Playables;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateHallResolved: GameBaseState
    {
        [SerializeField] private VoiceLineSO _openWalletVoiceLine;
        [SerializeField] private PlayableAsset _openCaseAsset;

        private PlayableDirector _playableDirector;

        public override GameStatesIndex StateIndex { get; protected set; } = GameStatesIndex.GameStateHallResolved;

        public override void EnterState(GameStateManager context)
        {
            _playableDirector = GameObject.FindGameObjectWithTag("HallPlayableDirector").GetComponent<PlayableDirector>();

            if (!_playableDirector)
                throw new NullReferenceException("HallPlayableDirector not found");

            OpenCase();
            Narrator.Narrator.Instance.StartCoroutine(Narrator.Narrator.Instance.PlayVoiceLineWithDelay(_openWalletVoiceLine, 1f));
        }

        public override void UpdateState(GameStateManager context)
        {

        }

        public override void ExitState(GameStateManager context)
        {

        }

        private void OpenCase()
        {
           _playableDirector.playableAsset = _openCaseAsset;
           _playableDirector.extrapolationMode = DirectorWrapMode.Hold;
           _playableDirector.Play();
        }

    }
}
