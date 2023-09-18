using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay.BaseGameplayData;
using CryptoQuest.Gameplay.Battle.Core;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Battle.Core
{
    public class SkillInfoBuilder
    {
        private int _id;
        private LocalizedString _skillName;
        private LocalizedString _skillDescription;
        private Elemental _element;
        private SkillType _skillType;
        private SkillCategory _category;
        private float _cost;
        private SkillEffectType _effectType;
        private bool _isFixed;
        private SkillParameters _skillParameters;
        private AbilityUsageScenarioSO _usageScenario;

        public SkillInfo Build()
        {
            return new SkillInfo()
            {
                Id = _id,
                SkillName = _skillName,
                SkillDescription = _skillDescription,
                Element = _element,
                SkillType = _skillType,
                Category = _category,
                Cost = _cost,
                EffectType = _effectType,
                UsageScenarioSO = _usageScenario,
                SkillParameters = _skillParameters
            };
        }

        public SkillInfoBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public SkillInfoBuilder WithSkillName(LocalizedString skillName)
        {
            _skillName = skillName;
            return this;
        }

        public SkillInfoBuilder WithSkillDescription(LocalizedString skillDescription)
        {
            _skillDescription = skillDescription;
            return this;
        }

        public SkillInfoBuilder WithElement(Elemental element)
        {
            _element = element;
            return this;
        }

        public SkillInfoBuilder WithSkillType(SkillType skillType)
        {
            _skillType = skillType;
            return this;
        }

        public SkillInfoBuilder WithCategory(SkillCategory category)
        {
            _category = category;
            return this;
        }

        public SkillInfoBuilder WithMPConsumption(float cost)
        {
            _cost = cost;
            return this;
        }

        public SkillInfoBuilder WithEffectType(SkillEffectType effectType)
        {
            _effectType = effectType;
            return this;
        }

        public SkillInfoBuilder WithIsFixed(bool isFixed)
        {
            _isFixed = isFixed;
            return this;
        }

        public SkillInfoBuilder WithUsageScenario(AbilityUsageScenarioSO usageScenario)
        {
            _usageScenario = usageScenario;
            return this;
        }

        public SkillInfoBuilder WithSkillParameters(SkillParameters skillParameters)
        {
            _skillParameters = skillParameters;
            return this;
        }
    }
}