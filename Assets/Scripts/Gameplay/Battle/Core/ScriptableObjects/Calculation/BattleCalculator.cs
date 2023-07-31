using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation
{
    public class BattleCalculator
    {
        public static float CalculateBaseDamage(SkillParameters skillParameters, float attackPower, float modifierScale)
        {
            float damage =
                (skillParameters.BasePower + (attackPower - skillParameters.SkillPowerThreshold) *
                    skillParameters.PowerValueAdded);
            damage = Mathf.Clamp(damage, skillParameters.PowerLowerLimit, skillParameters.PowerUpperLimit);
            float baseDamage = damage + damage * modifierScale;
            return baseDamage;
        }

        public static float CalculateProbabilityOfRetreat(float targetMaxAttributeValue, float ownerAttributeValue)
        {
            return (50 - 50 * (targetMaxAttributeValue - ownerAttributeValue) / 100) / 100;
        }
    }
}