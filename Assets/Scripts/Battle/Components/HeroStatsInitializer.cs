using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Helper;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// Requires <see cref="HeroBehaviour"/> in order to init the stats because it use <see cref="StatsDef"/> instead
    /// </summary>
    public class HeroStatsInitializer : CharacterComponentBase
    {
        private HeroBehaviour _heroComponent;
        private readonly ILevelAttributeCalculator _levelAttributeCalculator = new DefaultLevelAttributeCalculator();

        private AttributeSystemBehaviour _attributeSystem;

        protected override void Awake()
        {
            base.Awake();
            _heroComponent = GetComponent<HeroBehaviour>();
            _attributeSystem = GetComponent<AttributeSystemBehaviour>();
        }

        /// <summary>
        /// Order matters
        /// </summary>
        public override void Init()
        {
            InitBaseStats();
            InitAllAttributes();

            _attributeSystem.UpdateAttributeValues(); // Update the current value
        }

        /// <summary>
        /// We will need a base stats such as STR, INT, DEX, etc. these need to init first
        ///
        /// Use the <see cref="StatsDef"/> which contains the base stats to init
        /// </summary>
        private void InitBaseStats()
        {
            var attributeDefs = _heroComponent.Stats.Attributes;
            var charLvl = _heroComponent.Level; // TODO: Use level system might be better
            var characterAllowedMaxLvl = _heroComponent.Stats.MaxLevel;
            for (int i = 0; i < attributeDefs.Length; i++)
            {
                var attributeDef = attributeDefs[i];
                _attributeSystem.AddAttribute(attributeDef.Attribute);
                var baseValueAtLevel =
                    _levelAttributeCalculator.GetValueAtLevel(charLvl, attributeDef, characterAllowedMaxLvl);
                _attributeSystem.SetAttributeBaseValue(attributeDef.Attribute, baseValueAtLevel);
            }

            _attributeSystem.UpdateAttributeValues();
        }

        /// <summary>
        /// Calculate init value for all attributes, HP, MP will be same as MaxHP, MaxMP
        ///
        /// ATK = STR, DEF = VIT, etc.
        /// </summary>
        private void InitAllAttributes()
        {
            for (int i = 0; i < _attributeSystem.AttributeValues.Count; i++)
            {
                var attributeValue = _attributeSystem.AttributeValues[i];
                _attributeSystem.AttributeValues[i] =
                    attributeValue.Attribute.CalculateInitialValue(attributeValue,
                        _attributeSystem.AttributeValues);
            }
        }
    }
}