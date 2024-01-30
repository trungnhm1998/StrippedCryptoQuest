using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Item;
using CryptoQuest.Item.Consumable;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using TinyMessenger;

namespace CryptoQuest.Battle.Events
{
    public class BattleEvent : ITinyMessage
    {
        public object Sender { get; } = null;
    }

    public class RepeatableCommandExecutedEvent : BattleEvent
    {
        public Components.Character Character { get; }
        public ICommand Command { get; }

        public RepeatableCommandExecutedEvent(Components.Character character, ICommand command)
        {
            Character = character;
            Command = command;
        }
    }

    public abstract class RetreatEvent : BattleEvent
    {
        public Components.Character Character { get; set; }
    }

    public class RetreatSucceedEvent : RetreatEvent { }

    public class RetreatFailedEvent : RetreatEvent { }

    public class RoundStartedEvent : BattleEvent
    {
        public int Round { get; set; }
    }

    public class StartAcceptCommand : BattleEvent
    {
        public RoundStartedEvent RoundStartedContext { get; set; }
    }

    public class RoundEndedEvent : BattleEvent { }

    public class BattleEndedEvent : BattleEvent
    {
        public Battlefield Battlefield { get; set; }
    }

    public class UnloadedBattle : BattleEvent { }

    public abstract class TurnResultEvent : BattleEvent { }

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

    public abstract class TurnEvent : BattleEvent
    {
        public Components.Character Character { get; }

        protected TurnEvent(Components.Character character)
        {
            Character = character;
        }
    }

    public class TurnStartedEvent : TurnEvent
    {
        public TurnStartedEvent(Components.Character character) : base(character) { }
    }

    public class TurnEndedEvent : TurnEvent
    {
        public TurnEndedEvent(Components.Character character) : base(character) { }
    }

    public class ExecutingCommandEvent : TurnEvent
    {
        public ExecutingCommandEvent(Components.Character character) : base(character) { }
    }

    /// <summary>
    ///  Currently support <see cref="TagScriptableObject"/> which might be wrong for some cases
    /// </summary>
    public class EffectEvent : LogEvent
    {
        public TagScriptableObject Tag { get; set; }
    }

    public class PlayVfxEvent : BattleEvent
    {
        public int VfxId { get; }
        public PlayVfxEvent(int vfxId) => VfxId = vfxId;
    }

    public class ConsumeItemFailEvent : BattleEvent { }

    public class EffectAddedEvent : EffectEvent { }

    public class EffectRemovedEvent : EffectEvent { }

    public class EffectAffectingEvent : EffectEvent { }


    public class ItemEvent : BattleEvent
    {
        public ConsumableInfo ItemInfo { get; set; }
    }

    public class EnemyFledEvent : BattleEvent
    {
        public AbilitySystemBehaviour enemySystemBehaviour { get; private set; }

        public EnemyFledEvent(AbilitySystemBehaviour enemySystemBehaviour)
        {
            this.enemySystemBehaviour = enemySystemBehaviour;
        }
    }

    public class SelectedItemEvent : ItemEvent { }

    public class CancelSelectedItemEvent : ItemEvent { }

    #region Log Events

    public abstract class LogEvent : BattleEvent
    {
        public Components.Character Character { get; set; }
    }

    public class NormalAttackEvent : LogEvent
    {
        public Components.Character Target { get; set; }
    }

    public class GuardedEvent : LogEvent { }

    public class MpNotEnoughEvent : LogEvent { }

    public class CastSkillFailedEvent : LogEvent
    {
        public CastSkillAbility Skill { get; private set; }

        public CastSkillFailedEvent(CastSkillAbility skill)
        {
            Skill = skill;
        }
    }

    public class MissedEvent : LogEvent
    {
        public bool IsDamage { get; private set; }

        public MissedEvent(bool isDamage = true)
        {
            IsDamage = isDamage;
        }
    }

    public class CastingSkillEvent : LogEvent
    {
        public CastSkillAbility Skill { get; set; }
    }

    public class ReflectDamageEvent : LogEvent { }

    public class CanNotEscapeEvent : LogEvent{}

    public class CastSkillEvent : LogEvent
    {
        public CastSkillAbility Skill { get; private set; }
        public AbilitySystemBehaviour[] Targets { get; private set; }

        public CastSkillEvent(CastSkillAbility skill, params AbilitySystemBehaviour[] targets)
        {
            Skill = skill;
            Targets = targets;
        }
    }

    public class DamageOverTimeEvent : LogEvent
    {
        public AttributeScriptableObject AffectingAttribute { get; set; }
        public float Magnitude { get; set; }
    }

    public class ConsumeItemEvent : LogEvent
    {
        public ConsumableInfo ItemInfo { get; set; }
        public Components.Character Target { get; set; }
    }


    public class CastInvalidEvent : LogEvent
    {
        public CastSkillAbilitySpec Skill { get; }
        public AbilitySystemBehaviour Target { get; }

        public CastInvalidEvent(CastSkillAbilitySpec skill, Components.Character character,
            AbilitySystemBehaviour target)
        {
            Skill = skill;
            Character = character;
            Target = target;
        }
    }

    public class AbsorbingEvent : LogEvent
    {
        public AttributeScriptableObject AbsorbingAttribute { get; set; }
        public float Value { get; set; }
    }

    public class CriticalHitEvent : LogEvent { }

    #endregion
}