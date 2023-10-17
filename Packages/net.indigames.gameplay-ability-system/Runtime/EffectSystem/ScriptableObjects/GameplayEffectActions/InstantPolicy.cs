using System;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions
{
    [Serializable]
    public class InstantPolicy : IGameplayEffectPolicy
    {
        /// <summary>
        /// It's mean instantly modify base of the attribute
        /// suitable for non-stats attribute like HP (not MaxHP)
        /// Attack can be treat as instant effect
        /// Enemy -> attack (effect) -> Player
        ///
        /// Based on GAS I would want Instant effect as a infinite effect but for now I will modify the base value
        /// </summary>
        public ActiveGameplayEffect CreateActiveEffect(GameplayEffectSpec inSpec)
            => new InstantActiveEffectPolicy(inSpec);
    }

    [Serializable]
    public class InstantActiveEffectPolicy : ActiveGameplayEffect
    {
        public InstantActiveEffectPolicy(GameplayEffectSpec inSpec) : base(inSpec) { }

        public override void ExecuteActiveEffect()
        {
            Debug.Log(
                $"DefaultEffectApplier::ApplyInstantEffect {Spec.Def.name} to system {Spec.Target.name}");
            var modifySuccess = false;
            for (var index = 0; index < Spec.Def.EffectDetails.Modifiers.Length; index++)
            {
                var modifier = Spec.Def.EffectDetails.Modifiers[index];
                var evalData = new GameplayModifierEvaluatedData()
                {
                    Attribute = modifier.Attribute,
                    OpType = modifier.OperationType,
                    Magnitude = Spec.GetModifierMagnitude(index)
                };
                modifySuccess |= InternalExecuteMod(Spec, evalData);
            }

            foreach (var evalData in ComputedModifiers) modifySuccess |= InternalExecuteMod(Spec, evalData);

            // after modify the attribute this effect is now expired
            // The system only care if effect is expired or not
            IsActive = false;
        }
    }
}