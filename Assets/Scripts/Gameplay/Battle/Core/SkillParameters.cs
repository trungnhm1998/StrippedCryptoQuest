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
    public struct SkillInfo
    {
        public int Id;
        public LocalizedString SkillName;
        public LocalizedString SkillDescription;
        public Element Element;
        public SkillType SkillType;
        public SkillCategory Category;
        public float Cost;
        public SkillEffectType EffectType;
        public bool isFixed;
        public SkillParameters SkillParameters;
    }
}