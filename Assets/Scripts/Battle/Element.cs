using UnityEngine;

namespace CryptoQuest.Battle
{
    public class Element : MonoBehaviour, IComponent
    {
        public void Init(ICharacter character)
        {
            var attributeSystem = character.Attributes;
            attributeSystem.AddAttribute(character.Element.AttackAttribute);
            attributeSystem.AddAttribute(character.Element.ResistanceAttribute);
            for (int i = 0; i < character.Element.Multipliers.Length; i++)
            {
                var elementMultiplier = character.Element.Multipliers[i];
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