using System.Collections.Generic;
using CryptoQuest.Battle.Components.SpecialSkillBehaviours;
using CharacterBehaviour = CryptoQuest.Battle.Components.Character;
using CryptoQuest.Gameplay.Loot;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Abilites/Steal Ability",
        fileName = "StealAbility")]
    public class StealAbility : CastSkillAbility
    {
        protected override GameplayAbilitySpec CreateAbility() => new StealAbilitySpec(this);
    }

    public class StealAbilitySpec : CastSkillAbilitySpec
    {
        private IStealerBehaviour _stealerBehaviour;

        public StealAbilitySpec(CastSkillAbility def) : base(def) { }

        protected override void InternalExecute(AbilitySystemBehaviour target)
        {
            var onwerCharacter = Owner.GetComponent<CharacterBehaviour>();
            var targerCharacter = target.GetComponent<CharacterBehaviour>();
            _stealerBehaviour = Owner.GetComponent<IStealerBehaviour>();

            _stealerBehaviour.StealTarget(targerCharacter);
        }
    }
}