using Code.Scripts.Source.GameFSM.States;
using Code.Scripts.Source.Managers;
using Code.Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.Source.GameFSM
{
    public class StatesTrigger : MonoBehaviour
    {
        public Animator animator;
        public UnityEvent OnTrigered;

        private bool _isTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_isTriggered) return;
            if (!other.CompareTag("MainCamera")) return;

            OnTrigered?.Invoke();
        }

        private void SwitchStates(GameBaseState state)
        {
            GameStateManager.Instance.SwitchState(state);

            CustomLogger.LogInfo("Switch States trigerred" + "current states : " + GameStateManager.Instance.CurrentState);
            _isTriggered = true;
        }

        public void HallToLounge()
        {
            if (GameStateManager.Instance.CurrentState == GameStateManager.Instance.GameStates.HallResolved)
            {
                SwitchStates(GameStateManager.Instance.GameStates.LoungeIntro);
                animator?.SetTrigger("Run");
                GameStateManager.Instance.GreenHouseIsLocked = false;
            }
        }

        public void LoungeToGreenhouse()
        {
            if (GameStateManager.Instance.CurrentState == GameStateManager.Instance.GameStates.LoungePhase1) SwitchStates(GameStateManager.Instance.GameStates.GreenhouseInProgress);
        }

        public void GreenhouseToLounge()
        {
            if (GameStateManager.Instance.CurrentState == GameStateManager.Instance.GameStates.GreenhouseResolved) SwitchStates(GameStateManager.Instance.GameStates.LoungePhase2);
        }
    }
}
