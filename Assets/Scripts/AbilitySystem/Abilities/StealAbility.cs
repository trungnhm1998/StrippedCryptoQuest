using CryptoQuest.Battle.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;
using CharacterBehaviour = CryptoQuest.Battle.Components.Character;

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
            var stealerBehaviour = Owner.GetComponent<IStealerBehaviour>();
            stealerBehaviour.Steal(target.GetComponent<CharacterBehaviour>());
        }
    }
}