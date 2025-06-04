namespace Code.Scripts.Source.GameFSM.States
{
    public enum GameStatesIndex
    {
        GameStateUninitialized,
        GameStateMainMenu,
        GameStateLaunch,
        GameStatePause,

        GameStateHallIntro,
        GameStateHallInProgress,
        GameStateHallResolved,

        GameStateLoungeIntro,
        GameStateLoungePhase1,
        GameStateLoungePhase2,
        GameStateLoungeResolved,

        GameStateBackyardTransition,
        GameStateGreenhouseIntro,
        GameStateGreenhouseInProgress,
        GameStateGreenhouseResolved,

        GameStateLaboratoryIntro,
        GameStateLaboratoryPhase1,
        GameStateLaboratoryPhase2,
        GameStateLaboratoryResolved,

        GameStateEscapeRun,
        GameStateGameOver
    }
}
