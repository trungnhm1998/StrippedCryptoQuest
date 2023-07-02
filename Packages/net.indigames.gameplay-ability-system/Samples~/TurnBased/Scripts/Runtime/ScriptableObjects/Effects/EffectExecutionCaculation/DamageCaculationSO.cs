using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Indigames.AbilitySystem.Sample
{
    [CreateAssetMenu(fileName = "DamageCaculation", menuName = "Indigames Ability System/Effects/Execution Calculations/Damage Caculation")]
    public class DamageCalculationSO : AbstractEffectExecutionCalculationSO
    {
        public AttributeScriptableObject OwnerAttack;
        public AttributeScriptableObject TargetHP;

        public override bool ExecuteImplementation(ref AbstractEffect effectSpec,
            ref EffectAttributeModifier[] modifiers)
        {
            effectSpec.Owner.AttributeSystem.GetAttributeValue(OwnerAttack, out var attackDamage);

            modifiers = effectSpec.EffectSO.EffectDetails.Modifiers;
            var damageValue = attackDamage.CurrentValue;

            for (var index = 0; index < modifiers.Length; index++)
            {
                var effectAttributeModifier = modifiers[index];
                if (effectAttributeModifier.AttributeSO != TargetHP) continue;

                var previousModifier = effectAttributeModifier;
                previousModifier.Value = damageValue * -1;
                modifiers[index] = previousModifier;
                Debug.Log($"DamageCalculationSO::ExecuteImplementation: damageValue[{damageValue}]");
                return true;
            }

            return false;
        }
    }
}