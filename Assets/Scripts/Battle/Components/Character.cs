using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Character.Attributes;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

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

        protected virtual void Awake()
        {
            _targetComponent = GetComponent<ITargeting>();
            _gas = GetComponent<AbilitySystemBehaviour>();
            _command = new NullCommand(this);
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

        private ICommand _command;

        public ICommand Command
        {
            get => _command;
            protected set => _command = value;
        }

        public void SetCommand(ICommand command)
        {
            _command = command;
        }

        public IEnumerator ExecuteCommand()
        {
            // this character could die during presentation phase
            if (IsValid() == false) yield break;
            yield return OnPreExecuteCommand();
            yield return _command.Execute(); // this should not be null
            yield return OnPostExecuteCommand();
        }

        protected virtual IEnumerator OnPostExecuteCommand()
        {
            _command = new NullCommand(this);
            yield return new WaitForSeconds(1f);
        }

        protected virtual IEnumerator OnPreExecuteCommand()
        {
            yield return new WaitForSeconds(1f);
        }

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

    public class NullCommand : ICommand
    {
        private readonly Character _character;

        public NullCommand(Character character)
        {
            _character = character;
        }

        public IEnumerator Execute()
        {
            Debug.LogWarning($"No command for {_character.gameObject.name}.");
            yield break;
        }
    }
}