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
        public override TagScriptableObject[] GrantedTags => _spec.Parameters.GrantedTags;
        [SerializeField] private int _turn;
        [SerializeField] private TurnBaseAction _turnBaseAction;
        [SerializeField] private EffectSpec _spec;
        [SerializeField] private AbilitySystemBehaviour _owner;

        private Components.Character _character;
        private CommandExecutor _commandExecutor;

        public TurnBaseActiveEffectSpec(TurnBaseAction turnBaseAction, EffectSpec spec,
            AbilitySystemBehaviour owner) : base(spec)
        {
            _turn = spec.Parameters.ContinuesTurn;
            _owner = owner;
            _spec = spec;
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
            if (_spec.AbilitySpec.AbilitySO != spec.AbilitySpec.AbilitySO) return false;
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

            LogAffectingStatus();
            ModifyOwnerAttribute();
            _turn--;
            Debug.Log($"TurnBaseAction::UpdateTurn::[{_spec.AbilitySpec.AbilitySO.name}] turn[{_turn}] left");
            if (_turn <= 0)
            {
                _spec.IsExpired = true;
            }
        }

        private void LogAffectingStatus()
        {
            var grantedTags = _spec.AbilitySpec.Def.Parameters.SkillParameters.GrantedTags;
            foreach (var tag in grantedTags)
            {
                if (tag.AffectMessage.IsEmpty) continue;
                BattleEventBus.RaiseEvent(new EffectAffectingEvent()
                {
                    Character = _owner.GetComponent<Components.Character>(),
                    Reason = tag.AffectMessage
                });
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

                Debug.Log($"TurnBaseEffect::Modify attribute {attribute.name} " +
                          $"\nbase value[{attributeValue.BaseValue}] " +
                          $"\ncurrentValue[{attributeValue.CurrentValue}]");
                _owner.AttributeSystem.SetAttributeBaseValue(attribute, attributeValue.BaseValue);
            }
        }

        public override bool CanApplyModifiersToAttributeSystem() => false;
    }
}