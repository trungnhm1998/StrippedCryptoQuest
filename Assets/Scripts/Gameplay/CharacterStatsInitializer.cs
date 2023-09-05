using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Inventory.Items;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public class CharacterStatsInitializer : MonoBehaviour, ICharacterComponent
    {
        private CharacterBehaviourBase _characterBehaviour;
        private readonly ILevelCalculator _levelCalculator = new DefaultAttributeFromLevelCalculator();

        public void Init(CharacterBehaviourBase characterBehaviourBase)
        {
            _characterBehaviour = characterBehaviourBase;
            InitStats();
        }

        /// <summary>
        /// Order matters
        /// </summary>
        private void InitStats()
        {
            InitBaseStats();
            InitAllAttributes();
            InitElementStats();

            _characterBehaviour.AttributeSystem.UpdateAttributeValues(); // Update the current value
        }

        /// <summary>
        /// We will need a base stats such as STR, INT, DEX, etc. these need to init first
        ///
        /// Use the <see cref="CharacterSpec.StatsDef"/> which contains the base stats to init
        /// </summary>
        private void InitBaseStats()
        {
            var attributeDefs = _characterBehaviour.Spec.StatsDef.Attributes;
            var charLvl = _characterBehaviour.Spec.Level;
            var characterAllowedMaxLvl = _characterBehaviour.Spec.StatsDef.MaxLevel;
            for (int i = 0; i < attributeDefs.Length; i++)
            {
                var attributeDef = attributeDefs[i];
                _characterBehaviour.AttributeSystem.AddAttribute(attributeDef.Attribute);
                var baseValueAtLevel =
                    _levelCalculator.GetValueAtLevel(charLvl, attributeDef, characterAllowedMaxLvl);
                _characterBehaviour.AttributeSystem.SetAttributeBaseValue(attributeDef.Attribute, baseValueAtLevel);
            }

            _characterBehaviour.AttributeSystem.UpdateAttributeValues();
        }

        /// <summary>
        /// Calculate init value for all attributes, HP, MP will be same as MaxHP, MaxMP
        ///
        /// ATK = STR, DEF = VIT, etc.
        /// </summary>
        private void InitAllAttributes()
        {
            for (int i = 0; i < _characterBehaviour.AttributeSystem.AttributeValues.Count; i++)
            {
                var attributeValue = _characterBehaviour.AttributeSystem.AttributeValues[i];
                _characterBehaviour.AttributeSystem.AttributeValues[i] =
                    attributeValue.Attribute.CalculateInitialValue(attributeValue,
                        _characterBehaviour.AttributeSystem.AttributeValues);
            }
        }

        private void InitElementStats()
        {
            _characterBehaviour.AttributeSystem.AddAttribute(_characterBehaviour.Element.AttackAttribute);
            _characterBehaviour.AttributeSystem.AddAttribute(_characterBehaviour.Element.ResistanceAttribute);
            for (int i = 0; i < _characterBehaviour.Element.Multipliers.Length; i++)
            {
                var elementMultiplier = _characterBehaviour.Element.Multipliers[i];
                _characterBehaviour.AttributeSystem.AddAttribute(elementMultiplier.Attribute);
                _characterBehaviour.AttributeSystem.SetAttributeBaseValue(elementMultiplier.Attribute,
                    elementMultiplier.Value);
            }

            _characterBehaviour.AttributeSystem.UpdateAttributeValues();
        }
    }
}