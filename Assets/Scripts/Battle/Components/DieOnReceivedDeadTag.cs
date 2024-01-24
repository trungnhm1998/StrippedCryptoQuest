using System.Linq;
using CryptoQuest.AbilitySystem;
using CryptoQuest.AbilitySystem.Attributes;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;

namespace CryptoQuest.Battle.Components
{
    public class DieOnReceivedDeadTag : CharacterComponentBase
    {
        private TagSystemBehaviour _tagSystem => Character.AbilitySystem.TagSystem;

        protected override void OnInit()
        {
            _tagSystem.TagAdded += DieWhenDeadTagAdded;
        }

        protected override void OnReset() => _tagSystem.TagAdded -= DieWhenDeadTagAdded;

        private void DieWhenDeadTagAdded(TagScriptableObject[] tagScriptableObjects)
        {
            if (tagScriptableObjects.Contains(TagsDef.Dead) == false) return;
            Character.AttributeSystem.SetAttributeValue(AttributeSets.Health, new AttributeValue()
            {
                Attribute = AttributeSets.Health
            });
        }
    }
}