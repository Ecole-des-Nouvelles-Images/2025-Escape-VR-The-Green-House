using System;
using UnityEngine;
using UnityEngine.InputSystem;

using Code.Scripts.Source.GameFSM;
using Code.Scripts.Source.GameFSM.States;
using Code.Scripts.Utils;

namespace Code.Scripts.Source.Managers
{
    public class GameStateManager: MonoBehaviourSingleton<GameStateManager>
    {
        [field: SerializeField] public GameStates GameStates { get; private set; } = new();
        public GameBaseState CurrentState { get; private set; }
        public GameBaseState PreviousState { get; private set; }

        public bool GamePaused { get; set; }

        public InputAction MenuButton { get; private set; }
        public InputAction MenuButtonInteraction { get; private set; }

        public static Action OnFirstSceneLoaded;

        private void Awake()
        {
            MenuButton = InputSystem.actions.FindAction("XRI Left/MenuButton", true);
            MenuButtonInteraction = InputSystem.actions.FindAction("XRI Left Interaction/MenuButton", true);
        }

        private void Start()
        {
            CurrentState = GameStates.Uninitialized;
            CurrentState.EnterState(this);
        }

        private void OnEnable()
        {
            OnFirstSceneLoaded += InitializeFSM;
        }

        private void OnDestroy()
        {
            OnFirstSceneLoaded -= InitializeFSM;
        }

        private void OnApplicationQuit()
        {
            Destroy(this);
        }

        private void Update()
        {
            bool pauseButtonPressed = MenuButton.WasPressedThisFrame() || MenuButtonInteraction.WasPressedThisFrame();

            if (pauseButtonPressed && !GamePaused)
            {
                PauseGame();
                return;
            }

            CurrentState.UpdateState(this);
        }

        // ---

        private void InitializeFSM()
        {
            SwitchState(GameStates.MainMenu);
        }

        // TODO: rework GameStates to initialize inside their first EnterState() instead of using a bypass here.
        public void SwitchState(GameBaseState newState, bool bypassEntry = false, bool bypassExit = false)
        {
            PreviousState = CurrentState;

            if (newState == null || CurrentState == newState)
            {
                Debug.LogWarning("[GameStateManager] Warning: switching to " + (newState == null ? "null state" : $"same state {{{CurrentState}}}"));
                return;
            }

            if (!bypassExit && CurrentState != null)
                CurrentState.ExitState(this);

            CurrentState = newState;

            newState.EnterState(this);
        }

        public void PauseGame()
        {
            SwitchState(GameStates.Pause, false, true);
        }
    }
}
