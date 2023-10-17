using System;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Item;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
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

    public class BattleRetreatedEvent : BattleEndedEvent { }

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

    public class HighlightEnemyEvent : BattleEvent
    {
        public EnemyBehaviour Enemy { get; set; }
    }

    public class ResetHighlightEnemyEvent : BattleEvent { }

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

    public class TurnStartedEvent : BattleEvent
    {
        public Components.Character Character { get; set; }
    }
    
    public class ExecutingCommandEvent : BattleEvent
    {
        public Components.Character Character { get; set; }
    }

    public class NormalAttackEvent : LogEvent
    {
        public Components.Character Target { get; set; }
    }

    public class GuardedEvent : LogEvent { }

    public class MpNotEnoughEvent : LogEvent { }

    public class CastSkillFailedEvent : LogEvent { }

    public class MissedEvent : LogEvent { }

    public class CastSkillEvent : LogEvent
    {
        public CastSkillAbility Skill { get; set; }
    }

    public class CastSkillEffectEvent : LogEvent
    {
        public CastSkillAbility Skill { get; set; }
        public Components.Character Target { get; set; }
        public Action OnComplete { get; set; }
    }

    public class ConsumeItemEvent : LogEvent
    {
        public ConsumableInfo ItemInfo { get; set; }
        public Components.Character Target { get; set; }
    }

    /// <summary>
    ///  Currently support <see cref="TagScriptableObject"/> which might be wrong for some cases
    /// </summary>
    public class EffectEvent : LogEvent
    {
        public TagScriptableObject Tag { get; set; }
    }

    public class EffectAddedEvent : EffectEvent { }

    public class EffectRemovedEvent : EffectEvent { }

    public class EffectAffectingEvent : EffectEvent { }


    public class ItemEvent : BattleEvent
    {
        public ConsumableInfo ItemInfo { get; set; }
    }

    public class SelectedItemEvent : ItemEvent { }

    public class CancelSelectedItemEvent : ItemEvent { }
}