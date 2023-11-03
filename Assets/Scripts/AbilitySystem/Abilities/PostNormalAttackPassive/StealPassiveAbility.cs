using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using CharacterBehaviour = CryptoQuest.Battle.Components.Character;
using UnityEngine;
using CryptoQuest.Battle.Components.SpecialSkillBehaviours;

namespace CryptoQuest.AbilitySystem.Abilities.PostNormalAttackPassive
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Passive/Condition Skill/Steal", fileName = "ConditionSkill")]
    public class StealPassiveAbility : PostNormalAttackPassiveBase
    {
        protected override GameplayAbilitySpec CreateAbility()
            => new StealPassiveSpec();
    }

    public class StealPassiveSpec : PostNormalAttackPassiveSpecBase
    {
        private ActiveGameplayEffect _activeGameplayEffect;

        protected override void OnAttacked(DamageContext postAttackContext)
        {
            var target = postAttackContext.Target;
            if (!IsTargetValid(target)) return;
            
            var stealerBehaviour = Owner.GetComponent<IStealerBehaviour>();

            stealerBehaviour.StealTarget(target);

        }
    }
}