using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;

namespace CryptoQuest.Battle.Character
{
    public class AttributeChangeEvent : AttributesEventBase
    {
        public delegate void ChangeEvent(AttributeSystemBehaviour owner, AttributeValue oldValue,
            AttributeValue newValue);

        public event ChangeEvent Changed;

        public override void PreAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue newAttributeValue) { }

        public override void PostAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue oldAttributeValue,
            ref AttributeValue newAttributeValue) =>
            Changed?.Invoke(attributeSystem, oldAttributeValue, newAttributeValue);
    }
}