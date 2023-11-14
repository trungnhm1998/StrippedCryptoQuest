using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Abilities/Cast Skill With Effect",
        fileName = "CastEffectsOnTargetAbility")]
    public class CastEffectsOnTargetAbility : CastSkillAbility
    {
        [SerializeField] private GameplayEffectDefinition[] _effects = Array.Empty<GameplayEffectDefinition>();
        public GameplayEffectDefinition[] Effects => _effects;

        protected override GameplayAbilitySpec CreateAbility() =>
            new CastEffectsOnTargetAbilitySpec(this);
    }

    public class CastEffectsOnTargetAbilitySpec : CastSkillAbilitySpec
    {
        private readonly CastEffectsOnTargetAbility _def;
        private List<GameplayEffectSpec> _effectSpecs = new();

        public CastEffectsOnTargetAbilitySpec(CastEffectsOnTargetAbility def) : base(def)
        {
            _def = def;
        }

        protected override void InternalExecute(AbilitySystemBehaviour target)
        {
            foreach (var effectDef in _def.Effects)
            {
                var gameplayEffectSpec = CreateEffectSpec(effectDef);
                NotifyCastByTagCondition(target, effectDef.ApplicationTagRequirements.IgnoreTags);
                _effectSpecs.Add(gameplayEffectSpec);
                target.ApplyEffectSpecToSelf(gameplayEffectSpec);
            }
        }

        private GameplayEffectSpec CreateEffectSpec(GameplayEffectDefinition def) =>
            def.CreateEffectSpec(Owner, new GameplayEffectContextHandle(_def.Context));

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