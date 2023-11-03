using System.Collections;
using CryptoQuest.Battle.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities.PostNormalAttackPassive
{
    public struct DamageContext
    {
        public Battle.Components.Character Attacker;
        public Battle.Components.Character Target;
        public float Damage;
    }

    public class PostDamageContext : GameplayEffectContext
    {
        public PostDamageContext(GameplayEffectContext ctx) : base(ctx.SkillInfo) { }
        public DamageContext DamageContext { get; set; }
    }
    
    public abstract class PostNormalAttackPassiveBase : PassiveAbility
    {
        [field: SerializeField, Range(0, 1f)] public float SuccessRate { get; private set; } = 1f;
    }

    public abstract class PostNormalAttackPassiveSpecBase : PassiveAbilitySpec
    {
        protected override IEnumerator OnAbilityActive()
        {
            if (Character.TryGetComponent(out NormalAttack normalAttack))
                normalAttack.Attacked += NotifyPostAttack;
            yield break;
        }

        protected override void OnAbilityEnded()
        {
            if (Character.TryGetComponent(out NormalAttack normalAttack))
                normalAttack.Attacked -= NotifyPostAttack;
        }

        private void NotifyPostAttack(Battle.Components.Character attacker, Battle.Components.Character target,
            float damage) =>
            OnAttacked(new DamageContext()
            {
                Attacker = attacker,
                Target = target,
                Damage = damage
            });

        protected abstract void OnAttacked(DamageContext postAttackContext);

        protected bool FailedToActive(PostNormalAttackPassiveBase ability)
        {
            var rnd = Random.value;
            var failedToActive = rnd > ability.SuccessRate;
            Debug.Log(
                $"Failed to active {ability.name} with rate {ability.SuccessRate}, rolled {rnd}: {failedToActive}");
                
            return failedToActive;
        }

        protected bool IsTargetValid(Battle.Components.Character target)
        {
            if (!target.IsValidAndAlive())
            {
                Debug.Log($"Active {AbilitySO.name} fail because target is invalid or dead");
                return false;
            }
            return true;
        }

        protected void RemovePreviousEffectIfExisted(ref ActiveGameplayEffect effect)
        {
            if (effect == null) return;
            effect.IsActive = false;
            Owner.EffectSystem.RemoveEffect(effect.Spec);
            effect = null;
        }
    }
}