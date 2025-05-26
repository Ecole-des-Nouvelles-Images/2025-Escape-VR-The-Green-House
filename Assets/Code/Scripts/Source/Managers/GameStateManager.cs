using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

using Code.Scripts.Source.GameFSM;
using Code.Scripts.Source.GameFSM.States;
using Code.Scripts.Utils;
using Code.Scripts.Source.Types;
using Unity.XR.CoreUtils;

namespace Code.Scripts.Source.Managers
{
    public class GameStateManager: MonoBehaviourSingleton<GameStateManager>
    {
        public Action OnFirstSceneLoaded;

        [SerializeField] private XROrigin _playerXROrigin;
        [field: SerializeField] public GameStates GameStates { get; private set; } = new();

        public GameBaseState CurrentState { get; private set; }
        public GameBaseState PreviousState { get; private set; }

        public bool GamePaused { get; set; }

        public InputAction MenuButton { get; private set; }
        public InputAction MenuButtonInteraction { get; private set; }

        private List<NearFarInteractor> _xrNearFarInteractors;

        // ---

        private void Awake()
        {
            MenuButton = InputSystem.actions.FindAction("XRI Left/MenuButton", true);
            MenuButtonInteraction = InputSystem.actions.FindAction("XRI Left Interaction/MenuButton", true);
            _xrNearFarInteractors = new (FindObjectsByType<NearFarInteractor>(FindObjectsSortMode.None));
            ChangeNearFarInteractionMode(NearFarMode.None);
        }

        private void Start()
        {
            RecenterPlayerXROrigin();
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

            if (pauseButtonPressed)
            {
                PauseGame();
                return;
            }

            CurrentState.UpdateState(this);
        }

        // ---

        private void InitializeFSM()
        {
            Debug.Log("[GameStateManager] Initializing FSM...");
            Instance.SwitchState(Instance.GameStates.MainMenu);
        }

        // ---

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
            {
                Debug.Log($"[Manual] Current exiting state: {CurrentState.GetType().Name}");
                CurrentState.ExitState(this);
            }

            CurrentState = newState;

            if (!bypassEntry && CurrentState != null)
                newState.EnterState(this);
        }

        public void PauseGame()
        {
            if (!GamePaused)
                SwitchState(GameStates.Pause, false, true);
            else if (PreviousState == GameStates.Pause)
                throw new Exception($"[GameStateManager] Previous stored state is still in \"Pause\". Please verify transition from and before {CurrentState}.");
            else
                SwitchState(PreviousState, true, false);
        }

        public void ChangeNearFarInteractionMode(NearFarMode mode)
        {
            bool enableFarCast;
            bool enableNearCast;

            switch (mode) {
                case NearFarMode.None:
                    enableFarCast = false;
                    enableNearCast = false;
                    break;
                case NearFarMode.Far:
                    enableFarCast = true;
                    enableNearCast = false;
                    break;
                case NearFarMode.Near:
                    enableNearCast = true;
                    enableFarCast = false;
                    break;
                case NearFarMode.Both:
                    enableNearCast = true;
                    enableFarCast = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unhandled mode {mode} of NearFarMode type.");
            }

            foreach (NearFarInteractor interactor in _xrNearFarInteractors)
            {
                interactor.enableNearCasting = enableNearCast;
                interactor.enableFarCasting = enableFarCast;
            }

            Debug.Log($">> NearFar interaction mode changed to {mode}");
        }

        public void RecenterPlayerXROrigin()
        {
            _playerXROrigin.MatchOriginUpOriginForward(_playerXROrigin.transform.up, _playerXROrigin.transform.forward);
            Debug.Log("[GameStateManager] Player XROrigin re-centered.");
        }
    }
}
