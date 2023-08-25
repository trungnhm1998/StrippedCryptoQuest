using CryptoQuest.Gameplay.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public class CharacterStatsInitializer : MonoBehaviour, IStatInitializer
    {
        [SerializeField] private CharacterBehaviour _characterBehaviour;

        public void InitStats()
        {
            InitBaseStats();
            InitAllAttributes();
            InitElementStats();

            _characterBehaviour.AttributeSystem.UpdateAttributeValues(); // Update the current value
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

        /// <summary>
        /// We will need a base stats such as STR, INT, DEX, etc. these need to init first
        ///
        /// Use the <see cref="CharacterSpec.StatsDef"/> which contains the base stats to init
        /// </summary>
        private void InitBaseStats()
        {
            var attributeDefs = _characterBehaviour.Spec.StatsDef.Attributes;
            var charLvl = _characterBehaviour.Spec.Level;
            for (int i = 0; i < attributeDefs.Length; i++)
            {
                var attributeDef = attributeDefs[i];
                _characterBehaviour.AttributeSystem.AddAttribute(attributeDef.Attribute);
                var baseValueAtLevel = _characterBehaviour.Spec.GetValueAtLevel(charLvl, attributeDef);
                _characterBehaviour.AttributeSystem.SetAttributeBaseValue(attributeDef.Attribute, baseValueAtLevel);
            }

            _characterBehaviour.AttributeSystem.UpdateAttributeValues();
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