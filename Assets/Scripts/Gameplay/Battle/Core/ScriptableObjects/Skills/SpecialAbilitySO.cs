using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills
{
    public class SpecialAbilitySO : AbilitySO
    {
        public UnityAction<AbilityScriptableObject> OnAbilityActivated;
        protected override GameplayAbilitySpec CreateAbility() => new SpecialAbility(SkillInfo);

        public virtual void Activated()
        {
            OnAbilityActivated?.Invoke(this);
        }
    }

    public class SpecialAbility : Ability
    {
        protected new SpecialAbilitySO AbilitySO => (SpecialAbilitySO)_abilitySO;
        
        public SpecialAbility(SkillInfo skillInfo) : base(skillInfo) { }

        protected override IEnumerator InternalActiveAbility()
        {
            yield return AbilityActivated();
            EndAbility();
        }

        public virtual IEnumerator AbilityActivated()
        {
            AbilitySO.Activated();
            yield break;
        }

        public override bool CanActiveAbility()
        {
            return !IsActive && base.CanActiveAbility();
        }
    }
}