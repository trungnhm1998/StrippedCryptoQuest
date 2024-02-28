﻿using CryptoQuest.AbilitySystem.Attributes;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// Due to <see cref="ElementalAttribute.CalculateInitialValue"/> this component need to be call after <see cref="HeroStatsInitializer"/>
    /// </summary>
    public class Element : CharacterComponentBase
    {
        private Elemental _element;
        public Elemental ElementValue => _element;

        protected override void OnInit()
        {
            _element = Character.Element;
            var attributeSystem = Character.AttributeSystem;
            attributeSystem.AddAttribute(_element.AttackAttribute);
            attributeSystem.AddAttribute(_element.ResistanceAttribute);
            for (int i = 0; i < _element.Multipliers.Length; i++)
            {
                var elementMultiplier = _element.Multipliers[i];
                attributeSystem.AddAttribute(elementMultiplier.Attribute);
                // TODO: This will call UpdateAttributeValues() optimize
                attributeSystem.SetAttributeBaseValue(elementMultiplier.Attribute,
                    elementMultiplier.Value);
            }

            // TODO: Optimize
            attributeSystem.UpdateAttributeValues();
        }
    }
}