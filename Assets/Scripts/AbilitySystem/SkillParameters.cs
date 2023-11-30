using System;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.AbilitySystem
{
    [Serializable]
    public class SkillParameters
    {
        public Elemental Element;
        public float BasePower;
        public float PowerUpperLimit;
        public float PowerLowerLimit;
        public float SkillPowerThreshold;
        public float PowerValueAdded;
        public float PowerValueReduced;
        public bool IsFixed;
        public int ContinuesTurn = 1;
        public EEffectType EffectType;

        [FormerlySerializedAs("targetAttribute")]
        public CustomExecutionAttributeCaptureDef TargetAttribute;
    }

    [Serializable]
    public struct SkillInfo
    {
        public int Id;
        public Sprite SkillIcon;
        public ESkillType SkillType;
        public SkillCategory Category;
        public float Cost;
        public EAbilityUsageScenario UsageScenarioSO;
        public int VfxId;
        public SkillParameters SkillParameters;

        public bool CheckUsageScenario(EAbilityUsageScenario usageScenario)
        {
            return UsageScenarioSO.HasFlag(usageScenario);
        }
    }
}