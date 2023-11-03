using System;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem;
using CryptoQuest.AbilitySystem.Attributes;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.Components
{
    [RequireComponent(typeof(AbilitySystemBehaviour))]
    public abstract class Character : MonoBehaviour
    {
        public event Action TurnStarted;
        public event Action TurnEnded;

        #region GAS

        private AbilitySystemBehaviour _gas;
        public AttributeSystemBehaviour AttributeSystem => _gas.AttributeSystem;
        public EffectSystemBehaviour GameplayEffectSystem => _gas.EffectSystem;
        public AbilitySystemBehaviour AbilitySystem => _gas;

        #endregion

        private readonly Dictionary<Type, object> _cachedComponents = new();
        private ITargeting _targetComponent;
        public ITargeting Targeting => _targetComponent;
        private Elemental _element;
        public Elemental Element => _element;
        public abstract string DisplayName { get; }
        public abstract LocalizedString LocalizedName { get; }

        protected virtual void Awake()
        {
            _targetComponent = GetComponent<ITargeting>();
            _gas = GetComponent<AbilitySystemBehaviour>();
        }

        public void Init(Elemental element)
        {
            _element = element;
            AttributeSystem.Init();

            var components = GetComponents<CharacterComponentBase>();
            foreach (var comp in components)
                comp.Init();
        }

        private void OnDestroy()
        {
            if (IsValid() == false) return;
            var components = GetComponents<CharacterComponentBase>();
            foreach (var comp in components)
                comp.Reset();
        }

        public ActiveGameplayEffect ApplyEffect(GameplayEffectSpec effectSpec)
        {
            return AbilitySystem.ApplyEffectSpecToSelf(effectSpec);
        }

        public void RemoveEffect(GameplayEffectSpec activeEffectEffectSpec)
        {
            GameplayEffectSystem.RemoveEffect(activeEffectEffectSpec);
        }

        public bool HasTag(TagScriptableObject tagSO) => _gas.TagSystem.HasTag(tagSO);

        public abstract bool IsValid();
        public virtual bool IsValidAndAlive() => IsValid() && !HasTag(TagsDef.Dead);

        /// <summary>
        /// Same as Unity's <see cref="GameObject.TryGetComponent{T}(out T)"/> but with a cache
        /// </summary>
        public new bool TryGetComponent<T>(out T component) where T : class
        {
            var type = typeof(T);
            if (!_cachedComponents.TryGetValue(type, out var value))
            {
                if (base.TryGetComponent(out component))
                    _cachedComponents.Add(type, component);

                return component != null;
            }

            component = (T)value;
            return true;
        }

        public virtual void OnTurnStarted()
        {
            _gas.EffectSystem.UpdateAttributeModifiersUsingAppliedEffects(); // this also remove expired effects
            TurnStarted?.Invoke();
        }

        public virtual void OnTurnEnded() => TurnEnded?.Invoke();
    }
}