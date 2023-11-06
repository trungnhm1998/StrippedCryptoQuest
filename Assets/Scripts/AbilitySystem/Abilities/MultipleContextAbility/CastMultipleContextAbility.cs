using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities.MultipleContextAbility
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Abilities/Cast Skill With Multiple Context",
        fileName = "CastMultipleContextAbility")]
    public class CastMultipleContextEffectsAbility : CastSkillAbility
    {
        [SerializeField] private ContextEffectsMapping[] _contextEffectsMapping = Array.Empty<ContextEffectsMapping>();
        public ContextEffectsMapping[] EffectsMapping => _contextEffectsMapping;

        protected override GameplayAbilitySpec CreateAbility() =>
            new CastMultipleContextEffectsSpec(this);
    }

    public class CastMultipleContextEffectsSpec : CastSkillAbilitySpec
    {
        private readonly CastMultipleContextEffectsAbility _def;
        private List<GameplayEffectSpec> _effectSpecs = new();
        private ContextEffectsMapping _currentContextMapping;

        public CastMultipleContextEffectsSpec(CastMultipleContextEffectsAbility def) : base(def)
        {
            _def = def;
        }

        protected override void InternalExecute(AbilitySystemBehaviour target) 
        {
            if (_currentContextMapping == null) return;
            ApplyEffectsOnTarget(_currentContextMapping, target);
        }

        protected override void ExecuteAbility()
        {
            foreach (var effectMapping in _def.EffectsMapping)
            {
                if (effectMapping.TargetType == EEffectTargetType.SelfTarget)
                {
                    ApplyEffectsOnTarget(effectMapping, Owner);
                    continue;
                }

                _currentContextMapping = effectMapping;
                base.ExecuteAbility();
            }
        }

        private void ApplyEffectsOnTarget(ContextEffectsMapping effectsMapping, AbilitySystemBehaviour target)
        {
            var gameplayEffectSpecs = CreateEffectSpecs(effectsMapping);
            foreach (var spec in gameplayEffectSpecs)
            {
                _effectSpecs.Add(spec);
                target.ApplyEffectSpecToSelf(spec);
            }
        }

        private List<GameplayEffectSpec> CreateEffectSpecs(ContextEffectsMapping effectsMapping)
        {
            List<GameplayEffectSpec> specs = new();
            foreach (var effect in effectsMapping.Effects)
            {
                var spec = effect.CreateEffectSpec(Owner,
                    new GameplayEffectContextHandle(effectsMapping.ContextSelector.GetContext(_def)));
                specs.Add(spec);
            }
            return specs;
        }

        protected override void Cleanup()
        {
            foreach (var effectSpec in _effectSpecs)
            {
                effectSpec.IsExpired = true;
            }

            _effectSpecs.Clear();
        }
    }
}