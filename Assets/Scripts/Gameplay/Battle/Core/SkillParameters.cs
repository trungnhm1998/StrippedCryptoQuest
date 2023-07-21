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
        public float MPConsumption;
        public SkillEffectType EffectType;
        public bool isFixed;
        public SkillParameters SkillParameters;

        public SkillInfo(int id)
        {
            this.Id = id;
            this.SkillName = null;
            this.SkillDescription = null;
            this.Element = null;
            this.SkillType = null;
            this.Category = null;
            this.MPConsumption = 0;
            this.EffectType = null;
            this.isFixed = false;
            this.SkillParameters = null;
        }

        public static SkillInfo Create()
        {
            return new SkillInfo(0);
        }

        public SkillInfo WithId(int id)
        {
            this.Id = id;
            return this;
        }

        public SkillInfo WithSkillName(LocalizedString skillName)
        {
            this.SkillName = skillName;
            return this;
        }

        public SkillInfo WithDescription(LocalizedString skillDescription)
        {
            this.SkillDescription = skillDescription;
            return this;
        }

        public SkillInfo WithElement(Element element)
        {
            this.Element = element;
            return this;
        }

        public SkillInfo WithSkillType(SkillType skillType)
        {
            this.SkillType = skillType;
            return this;
        }

        public SkillInfo WithCategory(SkillCategory category)
        {
            this.Category = category;
            return this;
        }

        public SkillInfo WithMPConsumption(float mpConsumption)
        {
            this.MPConsumption = mpConsumption;
            return this;
        }

        public SkillInfo WithEffectType(SkillEffectType effectType)
        {
            this.EffectType = effectType;
            return this;
        }

        public SkillInfo WithIsFixed(bool isFixed)
        {
            this.isFixed = isFixed;
            return this;
        }

        public SkillInfo WithSkillParameters(SkillParameters skillParameters)
        {
            this.SkillParameters = skillParameters;
            return this;
        }
    }
}