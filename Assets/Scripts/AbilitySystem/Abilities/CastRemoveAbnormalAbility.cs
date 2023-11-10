using System;
using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Abilities/Cast Skill Remove Abnormal",
        fileName = "CastRemoveAbnormalAbility")]
    public class CastRemoveAbnormalAbility : CastEffectsOnTargetAbility
    {
        [field: SerializeField] public TagScriptableObject[] CancelEffectWithTags { get; private set; } =
            Array.Empty<TagScriptableObject>();

        protected override GameplayAbilitySpec CreateAbility() => new CastRemoveAbnormalAbilitySpec(this);
    }

    public class CastRemoveAbnormalAbilitySpec : CastEffectsOnTargetAbilitySpec
    {
        private readonly CastRemoveAbnormalAbility _def;
        public CastRemoveAbnormalAbilitySpec(CastRemoveAbnormalAbility def) : base(def) => _def = def;

        protected override void InternalExecute(AbilitySystemBehaviour target)
        {
            foreach (var tag in _def.CancelEffectWithTags)
            {
                target.EffectSystem.ExpireEffectWithTagImmediately(tag);
            }

            base.InternalExecute(target);
        }
    }
}