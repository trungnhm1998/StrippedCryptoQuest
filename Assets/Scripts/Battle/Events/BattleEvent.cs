using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Gameplay.Loot;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using TinyMessenger;

namespace CryptoQuest.Battle.Events
{
    public class BattleEvent : ITinyMessage
    {
        public object Sender { get; } = null;
    }

    public class RoundEndedEvent : BattleEvent { }

    public class BattleEndedEvent : BattleEvent
    {
        public Battlefield Battlefield { get; set; }
    }

    public class BattleWonEvent : BattleEndedEvent
    {
        public List<LootInfo> Loots { get; set; }
    }

    public class ForceWinBattleEvent : BattleEvent { }

    public class ForceLoseBattleEvent : BattleEvent { }

    public class BattleLostEvent : BattleEndedEvent { }

    public class BattleCleanUpFinishedEvent : BattleEvent { }

    public class LoadedEvent : BattleEvent { }
    public class UnloadingEvent : BattleEvent { }

    public class IndexEvent : BattleEvent
    {
        public int Index { get; set; }
    }

    public class SelectedDetailButtonEvent : IndexEvent { }

    public class DeSelectedDetailButtonEvent : IndexEvent { }

    public class HighlightHeroEvent : BattleEvent
    {
        public HeroBehaviour Hero { get; set; }
    }

    public class ShowPromptEvent : BattleEvent
    {
        public string Prompt { get; set; }
        public bool IsConcat { get; set; }
    }

    public class HidePromptEvent : BattleEvent { }

    public abstract class LogEvent : BattleEvent
    {
        public Components.Character Character { get; set; }
    }

    public class NormalAttackEvent : LogEvent
    {
        public Components.Character Target { get; set; }
    }
    public class GuardedEvent : LogEvent { }

    public class EffectExpired : LogEvent
    {
        public ActiveEffectSpecification Effect { get; set; }
    }

    public class MpNotEnoughEvent : LogEvent { }

    public class CastSkillFailedEvent : LogEvent { }

    public class MissedEvent : LogEvent { }
}