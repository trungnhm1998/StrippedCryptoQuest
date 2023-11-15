using System;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using TinyMessenger;

namespace CryptoQuest.AbilitySystem.EffectActions
{
    /// <summary>
    /// The effect will be active to the end of the battle
    /// </summary>
    [Serializable]
    public class BattleBasePolicy : IGameplayEffectPolicy
    {
        public virtual ActiveGameplayEffect CreateActiveEffect(GameplayEffectSpec inSpec) =>
            new BattleBasePolicyActiveEffect(this, inSpec);
    }

    [Serializable]
    public class BattleBasePolicyActiveEffect : ActiveGameplayEffect
    {
        private BattleBasePolicy _policyDef;
        private readonly TinyMessageSubscriptionToken _unloadingEvent;

        public BattleBasePolicyActiveEffect(BattleBasePolicy policyDef,
            GameplayEffectSpec spec) : base(spec)
        {
            _unloadingEvent = BattleEventBus.SubscribeEvent<UnloadingEvent>(ExpiredEffect);
            spec.Target.TagSystem.TagAdded += ExpiredEffectWhenTargetDie;
        }

        public override void OnRemoved()
        { 
            IsActive = false;
            RemoveEvents();
        }

        private void RemoveEvents()
        {
            Spec.Target.TagSystem.TagAdded -= ExpiredEffectWhenTargetDie;
            BattleEventBus.UnsubscribeEvent(_unloadingEvent);
        }

        private void ExpiredEffect(UnloadingEvent ctx) => RemoveEffect();

        /// <summary>
        /// DRY with turn base and other effect, we can use tag of some setting 
        /// to remove the effect when target die or when battle end
        /// </summary>
        /// <param name="tag"></param>
        private void ExpiredEffectWhenTargetDie(TagScriptableObject[] tag)
        {
            if (tag.Length == 0) return;
            if (tag[0] != TagsDef.Dead) return;
            RemoveEffect();
        }

        private void RemoveEffect()
        {
            Spec.Target.EffectSystem.RemoveEffect(Spec);
            OnRemoved();
        }
    }
}