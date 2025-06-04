namespace Code.Scripts.Source.GameFSM.States
{
    public enum GameStatesIndex
    {
        Uninitialized = 0,
        MainMenu,
        Launch,
        Pause,

        HallIntro,
        HallInProgress,
        HallResolved,

        LoungeIntro,
        LoungePhase1,
        LoungePhase2,
        LoungeResolved,

        BackyardTransition,
        GreenhouseIntro,
        GreenhouseInProgress,
        GreenhouseResolved,

        LaboratoryIntro,
        LaboratoryPhase1,
        LaboratoryPhase2,
        LaboratoryResolved,

        EscapeRun,
        GameOver
    }
}
