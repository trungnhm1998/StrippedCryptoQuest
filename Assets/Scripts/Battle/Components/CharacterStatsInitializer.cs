using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;

namespace CryptoQuest.Battle.Components
{
    public interface IStatsInitializer
    {
        public void SetStats(AttributeWithValue[] stats);
    }

    public class CharacterStatsInitializer : CharacterComponentBase, IStatsInitializer
    {
        private AttributeWithValue[] _stats = Array.Empty<AttributeWithValue>();

        /// <summary>
        /// Apply the stats to the target
        /// </summary>
        public override void Init()
        {
            var attributeSystem = Character.AttributeSystem;
            foreach (var stat in _stats)
                attributeSystem.SetAttributeBaseValue(stat.Attribute, stat.Value);
        }

        public void SetStats(AttributeWithValue[] stats) => _stats = stats;
    }
}