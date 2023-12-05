using System;
using CryptoQuest.AbilitySystem;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.UI.Utilities;
using IndiGames.Core.Common;
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
        public AttributeSystemBehaviour AttributeSystem => AbilitySystem.AttributeSystem;
        public EffectSystemBehaviour GameplayEffectSystem => AbilitySystem.EffectSystem;
        public AbilitySystemBehaviour AbilitySystem => _gas ??= GetComponent<AbilitySystemBehaviour>();

        #endregion

        private CacheableComponentGetter _componentGetter;

        private CacheableComponentGetter ComponentGetter =>
            _componentGetter ??= this.GetOrAddComponent<CacheableComponentGetter>();

        private ITargeting _targetComponent;
        public ITargeting Targeting => _targetComponent ??= GetComponent<ITargeting>();
        private Elemental _element;
        public Elemental Element => _element;
        public abstract string DisplayName { get; }
        public abstract LocalizedString LocalizedName { get; }

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
        public new bool TryGetComponent<T>(out T component)
            => ComponentGetter.TryGetComponent(out component);

        public new T GetComponent<T>()
            => ComponentGetter.GetComponent<T>();

        public virtual void OnTurnStarted()
        {
            _gas.EffectSystem.UpdateAttributeModifiersUsingAppliedEffects(); // this also remove expired effects
            TurnStarted?.Invoke();
        }

        public virtual void OnTurnEnded() => TurnEnded?.Invoke();
    }
}