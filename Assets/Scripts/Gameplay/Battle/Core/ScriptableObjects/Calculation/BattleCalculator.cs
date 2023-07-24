using CryptoQuest.Gameplay.Battle.Core;
using UnityEngine;

namespace CryptoQuest
{
    public class BattleCalculator
    {
        public float CalculateBaseDamage(SkillParameters skillParameters, float attackPower, float randomValue)
        {
            float damage =
                (skillParameters.BasePower + (attackPower - skillParameters.SkillPowerThreshold) *
                    skillParameters.PowerValueAdded);
            damage = damage < skillParameters.PowerLowerLimit ? skillParameters.PowerLowerLimit : damage;
            damage = damage > skillParameters.PowerUpperLimit ? skillParameters.PowerUpperLimit : damage;
            float baseDamage = damage + damage * randomValue;
            return baseDamage;
        }
    }
}