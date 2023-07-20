using System;
using CryptoQuest.Gameplay.BaseGameplayData;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
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
        public int ContinuesTurn = 1;
    }

    [Serializable]
    public class SkillInfo
    {
        public int Id;
        public LocalizedString SkillName;
        public LocalizedString SkillDescription;
        public Element Element;
        public SkillType SkillType;
        public SkillCategory Category;
        public float MPConsumption = 0;
        public SkillEffectType EffectType;
        public bool isFixed = true;
        public SkillParameters SkillParameters;
    }
}