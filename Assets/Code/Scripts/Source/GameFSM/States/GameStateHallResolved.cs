using System;
using Code.Scripts.Source.Managers;
using UnityEngine;
using UnityEngine.Playables;


namespace Code.Scripts.Source.GameFSM.States
{
    [Serializable]
    public class GameStateHallResolved: GameBaseState
    {
        private PlayableDirector _playableDirector;
        [SerializeField] private PlayableAsset _openCaseAsset;
        public override void EnterState(GameStateManager context)
        {
            base.EnterState(context);
            _playableDirector = GameObject.FindGameObjectWithTag("HallPlayableDirector").GetComponent<PlayableDirector>();
            if (!_playableDirector)
            {
                throw new NullReferenceException("HallPlayableDirector not found");
            }
            OpenCase();
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
