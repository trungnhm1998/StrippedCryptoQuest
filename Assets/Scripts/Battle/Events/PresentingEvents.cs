using CryptoQuest.Battle.Presenter.Commands;

namespace CryptoQuest.Battle.Events
{
    public class StartPresentingEvent : BattleEvent { }

    public class EnqueuePresentCommandEvent : BattleEvent
    {
        public IPresentCommand PresentCommand { get; private set; }

        public EnqueuePresentCommandEvent(IPresentCommand command)
        {
            PresentCommand = command;
        }
    }

    /// <summary>
    /// After all logs and vfx has been presented
    /// </summary>
    public class FinishedPresentingActionsEvent : BattleEvent { }

    /// <summary>
    /// raise this to handle change to the next state or end the battle accordingly
    /// </summary>
    public class FinishedPresentingEvent : BattleEvent { }

    public class ShakeUIEvent : BattleEvent { }

    public class LogDealtDamageEvent : BattleEvent
    {
        public Components.Character Character { get; private set; }

        public LogDealtDamageEvent(Components.Character character)
        {
            Character = character;
        }
    }

    public class HeroNormalAttackEvent : BattleEvent
    {
        public Components.Character Target { get; set; }
    }

    public class SealedEvent : EffectEvent { }

    public class FledEvent : BattleEvent
    {
        public Components.Character Character { get; private set; }
        public bool IsSuccess { get; private set; }

        public FledEvent(Components.Character character, bool isSuccess)
        {
            Character = character;
            IsSuccess = isSuccess;
        }
    }
}