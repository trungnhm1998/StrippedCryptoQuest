using System;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Battle.Core
{
    [Serializable]
    public class SkillParameters : AbilityParameters
    {
        public float BasePower;
        public float PowerUpperLimit;
        public float PowerLowerLimit;
        public float SkillPowerThreshold;
        public float PowerValueAdded;
        public float PowerValueReduced;
        public bool IsFixed;
        public int ContinuesTurn = 1;
        public EEffectType EffectType;
        public CustomExecutionAttributeCaptureDef targetAttribute;
    }

    [Serializable]
    public struct SkillInfo
    {
        public int Id;
        public LocalizedString SkillName;
        public LocalizedString SkillDescription;
        public Sprite SkillIcon;
        public Elemental Element;
        public SkillType SkillType;
        public SkillCategory Category;
        public float Cost;
        public SkillEffectType EffectType;
        public AbilityUsageScenarioSO UsageScenarioSO;
        // public BattleTargetTypeSO TargetType;
        public SkillParameters SkillParameters;

        public bool CheckUsageScenario(EAbilityUsageScenario usageScenario)
        {
            return (UsageScenarioSO != null) && (UsageScenarioSO.UsageScenario.HasFlag(usageScenario));
        }
    }
}