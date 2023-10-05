using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using UnityEngine;

namespace CryptoQuest.Battle.GameplayEffectActions
{
    [Serializable]
    public class TurnBaseAction : IGameplayEffectAction
    {
        [field: SerializeField] public int Turn { get; private set; }

        public ActiveEffectSpecification CreateActiveEffect(GameplayEffectSpec inSpec, AbilitySystemBehaviour owner)
        {
            return new TurnBaseActiveEffectSpec(this, inSpec, owner);
        }
    }

    [Serializable]
    public class TurnBaseActiveEffectSpec : ActiveEffectSpecification
    {
        [SerializeField] private int _turn;
        [SerializeField] private TurnBaseAction _turnBaseAction;
        [SerializeField] private GameplayEffectSpec _inSpec;
        [SerializeField] private AbilitySystemBehaviour _owner;

        private Components.Character _character;

        public TurnBaseActiveEffectSpec(TurnBaseAction turnBaseAction, GameplayEffectSpec inSpec,
            AbilitySystemBehaviour owner) : base(inSpec)
        {
            _turn = turnBaseAction.Turn;
            _owner = owner;
            _inSpec = inSpec;
            _turnBaseAction = turnBaseAction;
            _character = owner.GetComponent<Components.Character>();
            _character.OnTurnStarted += UpdateTurn;
        }

        protected override void OnRelease()
        {
            _character.OnTurnStarted -= UpdateTurn;
        }

        private void UpdateTurn()
        {
            if (_turn <= 0) return;
            ModifyOwnerAttribute();
            _turn--;
            if (_turn <= 0)
            {
                _inSpec.IsExpired = true;
            }
        }

        // TODO: Refactor this
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