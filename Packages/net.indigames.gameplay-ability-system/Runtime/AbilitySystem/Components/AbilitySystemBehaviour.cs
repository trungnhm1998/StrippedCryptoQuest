using System.Collections.Generic;
using System.Linq;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace IndiGames.GameplayAbilitySystem.AbilitySystem.Components
{
    [RequireComponent(typeof(AttributeSystemBehaviour))]
    [RequireComponent(typeof(EffectSystemBehaviour))]
    [RequireComponent(typeof(AbilityEffectBehaviour))]
    [RequireComponent(typeof(TagSystemBehaviour))]
    public partial class AbilitySystemBehaviour : MonoBehaviour
    {
        [SerializeField] private AttributeSystemBehaviour _attributeSystem;
        public AttributeSystemBehaviour AttributeSystem => _attributeSystem;

        [SerializeField] private EffectSystemBehaviour _effectSystem;
        public EffectSystemBehaviour EffectSystem => _effectSystem;

        [SerializeField] private AbilityEffectBehaviour _abilityEffectSystem;
        public AbilityEffectBehaviour AbilityEffectSystem => _abilityEffectSystem;

        [SerializeField] private TagSystemBehaviour _tagSystem;
        public TagSystemBehaviour TagSystem => _tagSystem;

        private AbilitySpecificationContainer _grantedAbilities = new();
        public AbilitySpecificationContainer GrantedAbilities => _grantedAbilities;


#if UNITY_EDITOR
        private void OnValidate()
        {
            ValidateComponents();
        }
#endif

        private void ValidateComponents()
        {
            _attributeSystem = GetComponent<AttributeSystemBehaviour>();
            _effectSystem = GetComponent<EffectSystemBehaviour>();
            _tagSystem = GetComponent<TagSystemBehaviour>();
            _abilityEffectSystem = GetComponent<AbilityEffectBehaviour>();
        }

        private void Awake()
        {
            ValidateComponents();
            // assert all components are not null
            Assert.IsNotNull(_attributeSystem, $"Attribute System is required!");
            Assert.IsNotNull(_effectSystem, $"Effect System is required!");
            Assert.IsNotNull(_abilityEffectSystem, $"Ability Effect System is required!");
            Assert.IsNotNull(_tagSystem, $"Tag System is required!");
            _effectSystem.InitSystem(this);
        }

        /// <summary>
        /// Add/Give/Grant ability to the system. Only ability that in the system can be active
        /// There's only 1 ability per system (no duplicate ability)
        ///
        /// <see cref="AbstractAbility.InternalActiveAbility"/> required this system to be enabled/active in order to start a coroutine
        /// </summary>
        /// <param name="abilityDef"></param>
        /// <returns>A <see cref="AbstractAbility"/> to handle (humble object) their ability logic</returns>
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