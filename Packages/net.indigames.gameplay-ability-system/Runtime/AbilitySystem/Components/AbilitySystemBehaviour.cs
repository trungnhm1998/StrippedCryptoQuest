using System;
using System.Collections.Generic;
using System.Linq;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
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

        public AttributeSystemBehaviour AttributeSystem
        {
            get => _attributeSystem;
            set => _attributeSystem = value;
        }

        [SerializeField] private EffectSystemBehaviour _effectSystem;

        public EffectSystemBehaviour EffectSystem
        {
            get => _effectSystem;
            set => _effectSystem = value;
        }

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
            if (!_attributeSystem) _attributeSystem = GetComponent<AttributeSystemBehaviour>();
            if (!_effectSystem) _effectSystem = GetComponent<EffectSystemBehaviour>();
            if (!_tagSystem) _tagSystem = GetComponent<TagSystemBehaviour>();
            if (!_abilityEffectSystem) _abilityEffectSystem = GetComponent<AbilityEffectBehaviour>();
        }

        private void Awake()
        {
            ValidateComponents();
            Assert.IsNotNull(_attributeSystem, $"Attribute System is required!");
            Assert.IsNotNull(_effectSystem, $"Effect System is required!");
            Assert.IsNotNull(_abilityEffectSystem, $"Ability Effect System is required!");
            Assert.IsNotNull(_tagSystem, $"Tag System is required!");

            _effectSystem.Owner = this; // prevent circular dependency
        }

        private void OnDestroy()
        {
            RemoveAllAbilities();
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
                throw new NullReferenceException("AbilitySystemBehaviour::GiveAbility::AbilityDef is null");

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

        public T GiveAbility<T>(AbilityScriptableObject abilityDef) where T : GameplayAbilitySpec
            => (T)GiveAbility(abilityDef); // can I somehow make this generic?

        private void OnGrantedAbility(GameplayAbilitySpec gameplayAbilitySpecSpec)
        {
            if (!gameplayAbilitySpecSpec.AbilitySO) return;
            Debug.Log(
                $"AbilitySystemBehaviour::OnGrantedAbility {gameplayAbilitySpecSpec.AbilitySO.name} to {gameObject.name}");
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
        /// TODO: Move this into <see cref="GameplayAbilitySpec"/>
        /// Create an effect spec from the effect definition, with this system as the source
        /// </summary>
        /// <param name="effectDef">The <see cref="EffectDefinition{T}"/> that are used to create the spec</param>
        /// <param name="context"></param>
        /// <returns>New effect spec based on the def</returns>
        public GameplayEffectSpec MakeOutgoingSpec(GameplayEffectDefinition effectDef,
            GameplayEffectContextHandle context = null)
        {
            if (effectDef == null)
                return new GameplayEffectSpec();

            if (context == null || context.IsValid())
                context = MakeEffectContext();

            return effectDef.CreateEffectSpec(this, context);
        }

        public GameplayEffectContextHandle MakeEffectContext()
        {
            var context = new GameplayEffectContextHandle(new GameplayEffectContext());
            context.Get().AddInstigator(gameObject);
            return context;
        }

        public ActiveGameplayEffect ApplyEffectSpecToSelf(GameplayEffectSpec effectSpec) =>
            _effectSystem.ApplyEffectToSelf(effectSpec);

        public bool CanApplyAttributeModifiers(GameplayEffectDefinition effectDef) =>
            _effectSystem.CanApplyAttributeModifiers(effectDef);

        public bool HasTag(TagScriptableObject gameplayTag) => _tagSystem.HasTag(gameplayTag);

        public GameplayAbilitySpec FindAbilitySpecFromDef(AbilityScriptableObject abilityDef)
            => _grantedAbilities.FirstOrDefault(spec => spec.AbilitySO == abilityDef);
    }
}