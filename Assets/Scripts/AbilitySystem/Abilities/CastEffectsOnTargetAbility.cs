using System;
using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Abilites/Cast Skill With Effect", fileName = "CastEffectsOnTargetAbility")]
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
        public CastEffectsOnTargetAbilitySpec(CastEffectsOnTargetAbility def) : base(def)
        {
            _def = def;
        }

        protected override IEnumerator InternalExecute(AbilitySystemBehaviour target)
        {
            foreach (var effectDef in _def.Effects)
            {
                target.ApplyEffectSpecToSelf(CreateEffectSpec(effectDef));
            }
            yield break;
        }
        

        private GameplayEffectSpec CreateEffectSpec(GameplayEffectDefinition def) =>
            def.CreateEffectSpec(Owner, new GameplayEffectContextHandle(_def.Context));
    }
}