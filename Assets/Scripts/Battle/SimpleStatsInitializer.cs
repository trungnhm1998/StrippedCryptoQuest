using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public interface IStatsInitializer
    {
        public void Init(AttributeSystemBehaviour attributeSystem);
    }

    public class SimpleStatsInitializer : MonoBehaviour, IStatsInitializer, IComponent
    {
        [SerializeField] private AttributeWithValue[] _stats = Array.Empty<AttributeWithValue>();

        public void Init(ICharacter character)
        {
            Init(character.Attributes);
        }

        public void Init(AttributeSystemBehaviour attributeSystem)
        {
            attributeSystem.Init();
            foreach (var stat in _stats)
                attributeSystem.SetAttributeBaseValue(stat.Attribute, stat.Value);
        }
    }
}