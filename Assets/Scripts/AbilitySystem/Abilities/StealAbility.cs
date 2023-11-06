using CryptoQuest.Battle.Components.SpecialSkillBehaviours;
using CharacterBehaviour = CryptoQuest.Battle.Components.Character;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Abilities/Steal Ability",
        fileName = "StealAbility")]
    public class StealAbility : CastSkillAbility
    {
        protected override GameplayAbilitySpec CreateAbility() => new StealAbilitySpec(this);
    }

    public class StealAbilitySpec : CastSkillAbilitySpec
    {
        public StealAbilitySpec(CastSkillAbility def) : base(def) { }

        protected override void InternalExecute(AbilitySystemBehaviour target)
        {
            var targetCharacter = target.GetComponent<CharacterBehaviour>();
            var stealerBehaviour = Owner.GetComponent<IStealerBehaviour>();

            stealerBehaviour.StealTarget(targetCharacter);
        }
    }
}