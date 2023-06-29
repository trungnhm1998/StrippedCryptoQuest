using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Indigames.AbilitySystem
{
    [RequireComponent(typeof(AttributeSystemBehaviour))]
    [RequireComponent(typeof(EffectSystemBehaviour))]
    [RequireComponent(typeof(TagSystemBehaviour))]
    public class AbilitySystemBehaviour : MonoBehaviour
    {
        [SerializeField] private AttributeSystemBehaviour _attributeSystem;
        public AttributeSystemBehaviour AttributeSystem => _attributeSystem;

        [SerializeField] private EffectSystemBehaviour _effectSystem;
        public EffectSystemBehaviour EffectSystem => _effectSystem;

        [SerializeField] private TagSystemBehaviour _tagSystem;
        public TagSystemBehaviour TagSystem => _tagSystem;

        protected AbilitySpecificationContainer _grantedAbilities = new();
        public AbilitySpecificationContainer GrantedAbilities => _grantedAbilities;


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_attributeSystem == null)
            {
                _attributeSystem = GetComponent<AttributeSystemBehaviour>();
            }
            if (_effectSystem == null)
            {
                _effectSystem = GetComponent<EffectSystemBehaviour>();
            }
            if (_tagSystem == null)
            {
                _tagSystem = GetComponent<TagSystemBehaviour>();
            }
        }
#endif

        private void Awake()
        {
            _effectSystem.InitSystem(this);
        }

        /// <summary>
        /// Add/Give/Grant ability to the system. Only ability that in the system can be active
        /// There's only 1 ability per system (no duplicate ability)
        /// </summary>
        /// <param name="abilityDef"></param>
        /// <returns>A ability handler (humble object) to execute their ability logic</returns>
        public AbstractAbility GiveAbility(AbilityScriptableObject abilityDef)
        {
            foreach (var ability in _grantedAbilities.Abilities)
            {
                if (ability.AbilitySO == abilityDef)
                    return ability;
            }

            var grantedAbility = abilityDef.GetAbilitySpec(this);

            return GiveAbility(grantedAbility);
        }

        public AbstractAbility GiveAbility(AbstractAbility inAbilitySpec)
        {
            if (!inAbilitySpec.AbilitySO) return null;

            foreach (AbstractAbility abilitySpec in _grantedAbilities.Abilities)
            {
                if (abilitySpec.AbilitySO == inAbilitySpec.AbilitySO)
                    return abilitySpec;
            }
            _grantedAbilities.Abilities.Add(inAbilitySpec);
            OnGrantedAbility(inAbilitySpec);

            return inAbilitySpec;
        }

        private void OnGrantedAbility(AbstractAbility abilitySpec)
        {
            if (!abilitySpec.AbilitySO) return;
            abilitySpec.Owner = this;
            abilitySpec.OnAbilityGranted(abilitySpec);
        }

        public void TryActiveAbility(AbstractAbility inAbilitySpec)
        {
            if (inAbilitySpec.AbilitySO == null) return;
            foreach (var abilitySpec in _grantedAbilities.Abilities)
            {
                if (abilitySpec != inAbilitySpec) continue;
                inAbilitySpec.ActivateAbility();
            }
        }

        public bool RemoveAbility(AbstractAbility ability)
        {
            List<AbstractAbility> grantedAbilitiesList = _grantedAbilities.Abilities;
            for (int i = grantedAbilitiesList.Count - 1; i >= 0; i--)
            {
                var abilitySpec = grantedAbilitiesList[i];
                if (abilitySpec == ability)
                {
                    OnRemoveAbility(abilitySpec);
                    grantedAbilitiesList.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
        
        private void OnRemoveAbility(AbstractAbility abilitySpec)
        {
            if (!abilitySpec.AbilitySO) return;

            abilitySpec.OnAbilityRemoved(abilitySpec);
        }
        
        public void RemoveAllAbilities()
        {
            for (int i = _grantedAbilities.Abilities.Count - 1; i >= 0; i--)
            {
                var abilitySpec = _grantedAbilities.Abilities[i];
                _grantedAbilities.Abilities.RemoveAt(i);
                OnRemoveAbility(abilitySpec);
            }
            _grantedAbilities.Abilities = new List<AbstractAbility>();
        }
        
        private void Update()
        {
            RemovePendingAbilities();
        }

        protected virtual void RemovePendingAbilities()
        {
            foreach (var ability in _grantedAbilities.Abilities.ToList())
            {
                if (ability.IsPendingRemove || ability.IsRemoveAfterActivation)
                    RemoveAbility(ability);
            }
        }
    }
}
