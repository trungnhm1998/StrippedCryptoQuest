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

        public static float CalculateEncounterRateBuff(float buff, float passiveBuff)
        {
            bool isZeroBuff = (buff == 0 && passiveBuff == 0);
            float buffDividen = isZeroBuff ? 1 : ((1 - buff) * (1 - passiveBuff));
            float buffRate = (buffDividen != 0) ? (1 / buffDividen) : 1;
            return buffRate;
        }
    }
}