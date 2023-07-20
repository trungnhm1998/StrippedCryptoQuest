using System.Collections;
using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills.CryptoQuestAbility;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills.CryptoQuestAbility
{
    public class SpecialAbilitySO : AbilitySO
    {
        public AbilityEventChannelSO AbilityEvent;
        protected override AbstractAbility CreateAbility() => new SpecialAbility(SkillInfo, AbilityEvent);
    }

    public class SpecialAbility : Ability
    {
        private AbilityEventChannelSO _abilityEvent;
        protected new SpecialAbilitySO AbilitySO => (SpecialAbilitySO)_abilitySO;

        public SpecialAbility(SkillInfo skillInfo, AbilityEventChannelSO abilityEvent) : base(
            skillInfo)
        {
            _abilityEvent = abilityEvent;
        }

        protected override IEnumerator InternalActiveAbility()
        {
            yield return base.InternalActiveAbility();
            _abilityEvent.EventRaised(_abilitySO);
            EndAbility();
        }

        public override bool CanActiveAbility()
        {
            return !IsActive && base.CanActiveAbility();
        }
    }
}