using System;
using UnityEngine;

using Code.Scripts.Source.GameFSM.States;

namespace Code.Scripts.Source.GameFSM
{
    [Serializable]
    public class GameStates
    {
        [field: SerializeField] public GameStateUninitialized Uninitialized { get; private set; } = new();
        [field: SerializeField] public GameStateMainMenu MainMenu { get; private set; } = new();
        [field: SerializeField] public GameStateLaunch Launch { get; private set; } = new();
        [field: SerializeField] public GameStatePause Pause { get; private set; } = new();

        [field: SerializeField] public GameStateHallIntro HallIntro { get; private set; } = new();
        [field: SerializeField] public GameStateHallInProgress HallInProgress { get; private set; } = new();
        [field: SerializeField] public GameStateHallResolved HallResolved { get; private set; } = new();

        [field: SerializeField] public GameStateLoungeIntro LoungeIntro { get; private set; } = new();
        [field: SerializeField] public GameStateLoungePhase1 LoungePhase1 { get; private set; } = new();
        [field: SerializeField] public GameStateLoungePhase2 LoungePhase2 { get; private set; } = new();
        [field: SerializeField] public GameStateLoungeResolved LoungeResolved { get; private set; } = new();

        [field: SerializeField] public GameStateBackyardTransition BackyardTransition { get; private set; } = new();
        [field: SerializeField] public GameStateGreenhouseIntro GreenhouseIntro { get; private set; } = new();
        [field: SerializeField] public GameStateGreenhouseInProgress GreenhouseInProgress { get; private set; } = new();
        [field: SerializeField] public GameStateGreenhouseResolved GreenhouseResolved { get; private set; } = new();

        [field: SerializeField] public GameStateLaboratoryIntro LaboratoryIntro { get; private set; } = new();
        [field: SerializeField] public GameStateLaboratoryPhase1 LaboratoryPhase1 { get; private set; } = new();
        [field: SerializeField] public GameStateLaboratoryPhase2 LaboratoryPhase2 { get; private set; } = new();
        [field: SerializeField] public GameStateLaboratoryResolved LaboratoryResolved { get; private set; } = new();

        [field: SerializeField] public GameStateEscapeRun Escape { get; private set; } = new();
        [field: SerializeField] public GameStateGameOver GameOver { get; private set; } = new();
    }
    
    
}
