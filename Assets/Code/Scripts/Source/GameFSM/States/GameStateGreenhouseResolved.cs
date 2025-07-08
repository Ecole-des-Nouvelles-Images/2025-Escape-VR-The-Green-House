using System;
using Code.Scripts.Source.Managers;
using UnityEngine;
using UnityEngine.Playables;

namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateGreenhouseResolved : GameBaseState
    {
        [SerializeField] private PlayableAsset _ivyStunnedAsset;

        private PlayableDirector _playableDirector;

        public override GameStatesIndex StateIndex { get; protected set; } = GameStatesIndex.GameStateGreenhouseResolved;

        public override void EnterState(GameStateManager context) {}

        public override void UpdateState(GameStateManager context)
        {
            /*_playableDirector = GameObject.FindGameObjectWithTag("HallPlayableDirector").GetComponent<PlayableDirector>();
            if (!_playableDirector)
            {
                throw new NullReferenceException("HallPlayableDirector not found");
            }
            IvySleep();*/
        }

        public override void ExitState(GameStateManager context) {}

        // ---

        private void IvySleep()
        {
            _playableDirector.playableAsset = _ivyStunnedAsset;
            _playableDirector.extrapolationMode = DirectorWrapMode.Hold;
            _playableDirector.Play();
        }
    }
}
