using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills.CryptoQuestAbility
{
    public class SpecialAbilitySO : AbilityScriptableObject
    {
        public int Id;
        public LocalizedString Name;
        public LocalizedString Description;
        public UnityAction<AbilityScriptableObject> OnAbilityActivated;
        protected override AbstractAbility CreateAbility() => new SpecialAbility();

        public virtual void Activated()
        {
            OnAbilityActivated?.Invoke(this);
        }
    }

    public class SpecialAbility : AbstractAbility
    {
        protected new SpecialAbilitySO AbilitySO => (SpecialAbilitySO)_abilitySO;

        protected override IEnumerator InternalActiveAbility()
        {
            yield return AbilityActivated();
            EndAbility();
        }

        public IEnumerator AbilityActivated()
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