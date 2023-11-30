using IndiGames.Core.Events;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;

namespace CryptoQuest.Battle.Actions
{
    public class StatsInitialized : ActionBase
    {
        public AttributeSystemBehaviour AttributeSystem { get; }
        public StatsInitialized(AttributeSystemBehaviour attributeSystem) => AttributeSystem = attributeSystem;
    }
}