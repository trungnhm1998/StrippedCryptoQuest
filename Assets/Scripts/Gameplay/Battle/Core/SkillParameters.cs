using System;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.TargetTypes;

namespace CryptoQuest.Gameplay.Battle.Core
{
    [Serializable]
    public class SkillParameters : AbilityParameters
    {
        public GenericData Element;
        public GenericData SkillType;
        public GenericData Category;
        public TargetType TargetType;
        public float MPConsumption = 0;
        public AttributeScriptableObject TargetAttribute;
        public TagScriptableObject TriggerTimming;
        public GenericData EffectType;
        public bool isFixed = true;
        public AbilityValues AbilityValues;
        public float SuccessRate;
        public GenericData UsageScenario;
        public int ContinuesTurn = 1;
    }

    [Serializable]
    public class AbilityValues
    {
        public float BasePower;
        public float PowerUpperLimit;
        public float PowerLowerLimit;
        public float SkillPowerThreshold;
        public float PowerValueAdded;
        public float PowerValueReduced;
    }
}