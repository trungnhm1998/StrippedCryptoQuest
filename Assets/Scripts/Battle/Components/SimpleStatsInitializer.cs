using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// Currently working with enemy because they doesn't has level
    ///
    /// this component will need <see cref="IStatsProvider"/> I don't care if it's error
    /// </summary>
    public class SimpleStatsInitializer : CharacterComponentBase
    {
        private AttributeWithValue[] _stats = Array.Empty<AttributeWithValue>();

        public override void Init()
        {
            _stats = GetComponent<IStatsProvider>().Stats;
            var attributeSystem = Character.AttributeSystem;
            foreach (var stat in _stats)
                attributeSystem.SetAttributeBaseValue(stat.Attribute, stat.Value);
        }
    }
}