using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using Unity.Collections;
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
        public delegate void AbilityGranted(GameplayAbilitySpec grantedAbility);
        public event AbilityGranted AbilityGrantedEvent;
        [SerializeField] private AttributeSystemBehaviour _attributeSystem;
        public AttributeSystemBehaviour AttributeSystem => _attributeSystem;

        [SerializeField] private EffectSystemBehaviour _effectSystem;
        public EffectSystemBehaviour EffectSystem => _effectSystem;

        [SerializeField] private AbilityEffectBehaviour _abilityEffectSystem;
        public AbilityEffectBehaviour AbilityEffectSystem => _abilityEffectSystem;

        [SerializeField] private TagSystemBehaviour _tagSystem;
        public TagSystemBehaviour TagSystem => _tagSystem;

        private List<GameplayAbilitySpec> _grantedAbilities = new();
        public IReadOnlyList<GameplayAbilitySpec> GrantedAbilities => _grantedAbilities;

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
            Assert.IsNotNull(_attributeSystem, $"Attribute System is required!");
            Assert.IsNotNull(_effectSystem, $"Effect System is required!");
            Assert.IsNotNull(_abilityEffectSystem, $"Ability Effect System is required!");
            Assert.IsNotNull(_tagSystem, $"Tag System is required!");
            _effectSystem.InitSystem(this);
        }

        /// <summary>
        /// Add/Give/Grant ability to the system. Only ability that in the system can be active
        /// There's only 1 ability per system (no duplicate ability)
        /// </summary>
        /// <param name="abilityDef"></param>
        /// <returns>A <see cref="GameplayAbilitySpec"/> to handle (humble object) their ability logic</returns>
        public GameplayAbilitySpec GiveAbility(AbilityScriptableObject abilityDef)
        {
            if (abilityDef == null)
            {
                Debug.LogWarning($"AbilitySystemBehaviour::GiveAbility::AbilityDef is null");
                return new GameplayAbilitySpec();
            }

            for (var index = 0; index < _grantedAbilities.Count; index++)
            {
                var ability = _grantedAbilities[index];
                if (ability.AbilitySO == abilityDef)
                    return ability;
            }

            var grantedAbility = abilityDef.GetAbilitySpec(this);

            _grantedAbilities.Add(grantedAbility);
            OnGrantedAbility(grantedAbility);

            return grantedAbility;
        }

        private void OnGrantedAbility(GameplayAbilitySpec gameplayAbilitySpecSpec)
        {
            if (!gameplayAbilitySpecSpec.AbilitySO) return;
            gameplayAbilitySpecSpec.OnAbilityGranted(gameplayAbilitySpecSpec);
            AbilityGrantedEvent?.Invoke(gameplayAbilitySpecSpec);
        }

        public void TryActiveAbility(GameplayAbilitySpec inGameplayAbilitySpecSpec)
        {
            if (inGameplayAbilitySpecSpec.AbilitySO == null) return;
            foreach (var abilitySpec in _grantedAbilities)
            {
                if (abilitySpec != inGameplayAbilitySpecSpec) continue;
                inGameplayAbilitySpecSpec.ActivateAbility();
            }
        }

        public bool RemoveAbility(GameplayAbilitySpec gameplayAbilitySpec)
        {
            List<GameplayAbilitySpec> grantedAbilitiesList = _grantedAbilities;
            for (int i = grantedAbilitiesList.Count - 1; i >= 0; i--)
            {
                var grantedSpec = grantedAbilitiesList[i];
                if (grantedSpec == gameplayAbilitySpec)
                {
                    OnRemoveAbility(gameplayAbilitySpec);
                    grantedAbilitiesList.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        private void OnRemoveAbility(GameplayAbilitySpec gameplayAbilitySpecSpec)
        {
            if (!gameplayAbilitySpecSpec.AbilitySO) return;

            gameplayAbilitySpecSpec.OnAbilityRemoved(gameplayAbilitySpecSpec);
        }

        public void RemoveAllAbilities()
        {
            for (int i = _grantedAbilities.Count - 1; i >= 0; i--)
            {
                var abilitySpec = _grantedAbilities[i];
                _grantedAbilities.RemoveAt(i);
                OnRemoveAbility(abilitySpec);
            }

            _grantedAbilities = new List<GameplayAbilitySpec>();
        }

        /// <summary>
        /// Create an effect spec from the effect definition, with this system as the source
        /// </summary>
        /// <param name="effectDef">The <see cref="EffectScriptableObject"/> that are used to create the spec</param>
        /// <returns>New effect spec based on the def</returns>
        public GameplayEffectSpec MakeOutgoingSpec(EffectScriptableObject effectDef)
        {
            if (effectDef == null)
                return new GameplayEffectSpec();

            GameplayEffectSpec effectSpecSpec = effectDef.CreateEffectSpec(this);
            return effectSpecSpec;
        }

        public GameplayEffectSpec ApplyEffectSpecToSelf(GameplayEffectSpec effectSpec)
        {
            return _effectSystem.ApplyEffectToSelf(effectSpec);
        }
    }
}