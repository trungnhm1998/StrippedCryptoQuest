using System;
using System.Collections.Generic;
using CryptoQuest.Character.Attributes;
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
        #region GAS

        private AbilitySystemBehaviour _gas;
        public AttributeSystemBehaviour AttributeSystem => _gas.AttributeSystem;
        public EffectSystemBehaviour GameplayEffectSystem => _gas.EffectSystem;
        public AbilitySystemBehaviour AbilitySystem => _gas;

        #endregion

        private readonly Dictionary<Type, Component> _cachedComponents = new();
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

        public ActiveEffectSpecification ApplyEffect(GameplayEffectSpec effectSpec)
        {
            return AbilitySystem.ApplyEffectSpecToSelf(effectSpec);
        }

        public void RemoveEffect(GameplayEffectSpec activeEffectEffectSpec)
        {
            GameplayEffectSystem.RemoveEffect(activeEffectEffectSpec);
        }

        public bool HasTag(TagScriptableObject tagSO) => _gas.TagSystem.HasTag(tagSO);

        public abstract bool IsValid();

        /// <summary>
        /// For enemy this will be random target, for hero this will be selected target if it's dead select next lowest hp target
        /// </summary>
        /// <param name="context">Holds the context of current battle, all heroes and enemies</param>
        public void UpdateTarget(BattleContext context) => Targeting.UpdateTargetIfNeeded(context);

        /// <summary>
        /// Same as Unity's <see cref="GameObject.TryGetComponent{T}(out T)"/> but with a cache
        /// </summary>
        public new bool TryGetComponent<T>(out T component) where T : Component
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
    }
}