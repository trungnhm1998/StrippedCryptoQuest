using System;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Character.Ability;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.GameplayEffectActions
{
    [Serializable]
    public class TurnBaseAction : IGameplayEffectAction
    {
        public ActiveEffectSpecification CreateActiveEffect(GameplayEffectSpec inSpec, AbilitySystemBehaviour owner)
        {
            return inSpec is not EffectSpec spec
                ? new ActiveEffectSpecification()
                : new TurnBaseActiveEffectSpec(this, spec, owner);
        }
    }

    [Serializable]
    public class TurnBaseActiveEffectSpec : ActiveEffectSpecification
    {
        public override TagScriptableObject[] GrantedTags => _inSpec.Parameters.GrantedTags;
        [SerializeField] private int _turn;
        [SerializeField] private TurnBaseAction _turnBaseAction;
        [SerializeField] private EffectSpec _inSpec;
        [SerializeField] private AbilitySystemBehaviour _owner;

        private Components.Character _character;
        private CommandExecutor _commandExecutor;

        public TurnBaseActiveEffectSpec(TurnBaseAction turnBaseAction, EffectSpec inSpec,
            AbilitySystemBehaviour owner) : base(inSpec)
        {
            _turn = inSpec.Parameters.ContinuesTurn;
            _owner = owner;
            _inSpec = inSpec;
            _turnBaseAction = turnBaseAction;
            _character = owner.GetComponent<Components.Character>();
            _character.TryGetComponent(out _commandExecutor);
            _commandExecutor.OnTurnStarted += UpdateTurn;
        }

        protected override void OnRelease()
        {
            _commandExecutor.OnTurnStarted -= UpdateTurn;
        }

        protected override void OnSpecStackChanged(GameplayEffectSpec otherSpec)
        {
            if (otherSpec is not EffectSpec spec) return;
            Debug.Log($"TurnBaseAction::OnStackChanged::from [{_turn}] to [{spec.Parameters.ContinuesTurn}] turn");
            _turn = spec.Parameters.ContinuesTurn;
        }

        public override bool AreEquals(GameplayEffectSpec inSpec)
        {
            var areEquals = base.AreEquals(inSpec);
            if (inSpec is not EffectSpec spec) return false;
            if (_inSpec.AbilitySpec.AbilitySO != spec.AbilitySpec.AbilitySO) return false;
            return areEquals;
        }

        private void UpdateTurn()
        {
            if (_turn <= 0 || Expired)
            {
                BattleEventBus.RaiseEvent(new EffectExpired()
                {
                    Character = _owner.GetComponent<Components.Character>(),
                    Effect = this
                });
                return;
            }

            ModifyOwnerAttribute();
            _turn--;
            Debug.Log($"TurnBaseAction::UpdateTurn::turn[{_turn}] left");
            if (_turn <= 0)
            {
                _inSpec.IsExpired = true;
            }
        }

        // TODO: DRY with instant effect, refactor
        private void ModifyOwnerAttribute()
        {
            for (int index = 0; index < ComputedModifiers.Count; index++)
            {
                var computedModifier = ComputedModifiers[index];
                var modifier = computedModifier.Modifier;
                var attribute = computedModifier.Attribute;

                // get a copy of the attribute value
                if (!_owner.AttributeSystem.TryGetAttributeValue(attribute, out var attributeValue)) continue;

                switch (computedModifier.ModifierType)
                {
                    case EAttributeModifierType.Add:
                        attributeValue.BaseValue += modifier.Additive;
                        break;
                    case EAttributeModifierType.Multiply:
                        attributeValue.BaseValue += attributeValue.BaseValue * modifier.Multiplicative;
                        break;
                    case EAttributeModifierType.Override:
                        attributeValue.BaseValue = modifier.Overriding;
                        break;
                }

                _owner.AttributeSystem.SetAttributeBaseValue(attribute, attributeValue.BaseValue);
                Debug.Log($"TurnBaseEffect::Modify attribute {attribute.name} " +
                          $"\nbase value[{attributeValue.BaseValue}] " +
                          $"\ncurrentValue[{attributeValue.CurrentValue}]");
            }
        }

        public override bool CanApplyModifiersToAttributeSystem() => false;
    }
}