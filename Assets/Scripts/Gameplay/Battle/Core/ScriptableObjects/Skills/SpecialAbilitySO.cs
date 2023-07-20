using System.Collections;
using CryptoQuest.Events;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills
{
    public class SpecialAbilitySO : InstantAbilitySO
    {
        public AbilityEventChannelSO AbilityEvent;
        protected override AbstractAbility CreateAbility() => new SpecialAbility(SkillParams, AbilityEvent);
    }

    public class SpecialAbility : InstantAbility
    {
        private AbilityEventChannelSO _abilityEvent;
        protected new SpecialAbilitySO AbilitySO => (SpecialAbilitySO)_abilitySO;

        public SpecialAbility(SkillParameters skillParameters, AbilityEventChannelSO abilityEvent) : base(
            skillParameters)
        {
            _abilityEvent = abilityEvent;
        }

        protected override IEnumerator InternalActiveAbility()
        {
            yield return base.InternalActiveAbility();
            EndAbility();
        }

        public override bool CanActiveAbility()
        {
            return !IsActive && base.CanActiveAbility();
        }
    }
}