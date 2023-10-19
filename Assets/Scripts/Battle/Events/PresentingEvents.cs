namespace CryptoQuest.Battle.Events
{
    public class StartPresentingEvent : BattleEvent { }

    /// <summary>
    /// After all logs and vfx has been presented
    /// </summary>
    public class FinishedPresentingActionsEvent : BattleEvent { }

    /// <summary>
    /// raise this to handle change to the next state or end the battle accordingly
    /// </summary>
    public class FinishedPresentingEvent : BattleEvent { }
}