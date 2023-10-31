using CryptoQuest.Battle.Presenter.Commands;
using CryptoQuest.Gameplay.Loot;

namespace CryptoQuest.Battle.Events
{
    public class CharacterTargetEvent : BattleEvent
    {
        public Components.Character Source { get; private set; }
        public Components.Character Target { get; private set; }

        public CharacterTargetEvent(Components.Character source,
            Components.Character target)
        {
            Source = source;
            Target = target;
        }
    }

    public class StealFailedEvent : CharacterTargetEvent
    {
        public StealFailedEvent(Components.Character stealer, 
            Components.Character target) : base(stealer, target) { }
    }

    public class StealSuccessEvent : CharacterTargetEvent
    {
        public StealableInfo StealableItem { get; private set;}

        public StealSuccessEvent(Components.Character stealer, 
            Components.Character target, StealableInfo stealableInfo) : base(stealer, target)
        {
            StealableItem = stealableInfo;
        }
}
}