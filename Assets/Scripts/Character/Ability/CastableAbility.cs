using System;
using System.Collections;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CryptoQuest.Character.Ability
{
    /// <summary>
    /// https://docs.google.com/spreadsheets/d/1EDrXrT1if63TLam1km_dLx2VvHt2rks2OWXEUq5OEPg/edit#gid=2122798992
    /// Base data class for character skill
    /// </summary>
    public class CastableAbility : AbilityScriptableObject
    {
        [field: SerializeField] public GameplayEffectDefinition Effect { get; private set; }

        /// <summary>
        /// The effect that cost to cast this ability
        /// The <see cref="GameplayEffectSpec.Source"/> is the caster
        /// </summary>
        public GameplayEffectDefinition Cost;

        [field: SerializeField] public SkillInfo Parameters { get; private set; }
        [field: SerializeField] public SkillTargetType TargetType { get; private set; }

        protected override GameplayAbilitySpec CreateAbility()
        {
            var ability = new CastableAbilitySpec(this);
            return ability;
        }
    }

    public class CastableAbilitySpec : GameplayAbilitySpec
    {
        public event Action NotEnoughResourcesToCast;
        private AbilitySystemBehaviour _targetSystem;
        private GameplayEffectDefinition _costEffect;
        private readonly CastableAbility _def;

        public CastableAbilitySpec(CastableAbility def)
        {
            _def = def;
        }

        public override void InitAbility(AbilitySystemBehaviour owner, AbilityScriptableObject abilitySO)
        {
            base.InitAbility(owner, abilitySO);

            if (_def.Cost == null) return;
            _costEffect = Object.Instantiate(_def.Cost);
            _costEffect.EffectDetails.Modifiers = new[]
            {
                new EffectAttributeModifier()
                {
                    Attribute = AttributeSets.Mana,
                    Value = -_def.Parameters.Cost
                }
            };
        }

        public override bool CanActiveAbility()
        {
            return base.CanActiveAbility() && CheckCost();
        }

        /// <summary>
        /// GameplayAbility.cpp::CheckCost line 906
        /// Create a cost effect spec, use the modifier and calculate the remaining resources, if the remaining resources
        /// is greater than 0 apply the effect
        /// </summary>
        /// <returns>Return true if after subtracted attribute that the cost needs greater than 0</returns>
        public bool CheckCost()
        {
            if (_costEffect == null) return true;
            if (Owner == null) return true;
            if (Owner.CanApplyAttributeModifiers(_costEffect)) return true;

            Debug.Log($"Not enough {_costEffect.EffectDetails.Modifiers[0].Attribute.name} to cast this ability");
            NotEnoughResourcesToCast?.Invoke();
            return false;
        }

        public void ApplyCost()
        {
            if (_costEffect == null) return;

            ApplyGameplayEffectToOwner(_costEffect);
        }

        private AbilitySystemBehaviour[] _targets;

        public void Execute(params AbilitySystemBehaviour[] characters)
        {
            _targets = characters;
            TryActiveAbility();
        }

        protected override IEnumerator OnAbilityActive()
        {
            ApplyCost();

            // TODO: REFACTOR GAS
            var spec = (CQEffectSpec)Owner.MakeOutgoingSpec(_def.Effect);
            spec.Parameters = _def.Parameters.SkillParameters;
            foreach (var target in _targets)
            {
                target.ApplyEffectSpecToSelf(spec);
            }

            yield break;
        }
    }
}