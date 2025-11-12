using Code.Scripts.Source.Managers;
using UnityEngine;

namespace Code.Scripts.Source.GameFSM.States
{
    public abstract class GameBaseState
    {
        public virtual void EnterState(GameStateManager context)
        {
            Debug.Log($"[{this.GetType().Name}] Entering state");
        }

        public abstract void UpdateState(GameStateManager context);

        public abstract void ExitState(GameStateManager context);

#if UNITY_EDITOR
        public override string ToString()
        {
            return this.GetType().Name;
        }
#endif
    }
}
