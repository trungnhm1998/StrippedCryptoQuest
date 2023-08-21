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

        public static float CalculatePercentPhysicalDamage(float baseDamage, float attack, float defence,
            float elementAttack, float elementResist)
        {
            float atkCorrection = BaseBattleVariable.CORRECTION_ATTACK_VALUE != 0
                ? BaseBattleVariable.CORRECTION_ATTACK_VALUE
                : 1;
            float defCorrection = BaseBattleVariable.CORRECTION_ATTACK_VALUE != 0
                ? BaseBattleVariable.CORRECTION_DEFENSE_VALUE
                : 1;
            float elementCorrection = elementResist != 0 ? elementAttack / elementResist : elementAttack;
            float damage = ((attack / atkCorrection) - (defence / defCorrection)) * baseDamage *
                           elementCorrection;
            damage = damage < 0 ? 0 : damage;
            Debug.Log("baseDamage " + baseDamage + "attack " + attack + "defence " + defence + "elementAttack " +
                      elementAttack + "elementResist " + elementResist + "damage " + damage);
            return damage;
        }

        public static float CalculateFixedPhysicalDamage(float baseDamage, float elementAttack, float elementResist)
        {
            float elementCorrection = elementResist != 0 ? elementAttack / elementResist : elementAttack;
            float damage = baseDamage * elementCorrection;
            damage = damage < 0 ? 0 : damage;
            Debug.Log("baseDamage " + baseDamage + "elementAttack " + elementAttack + "elementResist " + elementResist +
                      "damage " + damage);
            return damage;
        }
    }

    public static class BaseBattleVariable
    {
        public const float CORRECTION_ATTACK_VALUE = 2;
        public const float CORRECTION_DEFENSE_VALUE = 4;
        public const float CORRECTION_MAGIC_ATTACK_VALUE = 0.2f;
        public const float CORRECTION_ATTRIBUTE_VALUE = 1;
        public const float CORRECTION_PROBABILITY_VALUE = 100;
    }
}