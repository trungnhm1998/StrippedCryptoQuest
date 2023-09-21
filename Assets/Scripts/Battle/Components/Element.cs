﻿using CryptoQuest.Character.Attributes;

namespace CryptoQuest.Battle.Components
{
    public class Element : CharacterComponentBase
    {
        private Elemental _element;
        public Elemental ElementValue => _element;
        
        public void SetElement(Elemental element) => _element = element;

        public override void Init()
        {
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