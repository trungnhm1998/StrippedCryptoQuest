using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class InlineStatsProvider : MonoBehaviour, IStatsProvider
    {
        [field: SerializeField] public AttributeWithValue[] Stats { get; private set; } =
            Array.Empty<AttributeWithValue>();

        public void ProvideStats(AttributeWithValue[] attributeWithValues)
        {
            Stats = attributeWithValues;
        }
    }
}