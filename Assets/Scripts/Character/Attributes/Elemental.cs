using System;
using CryptoQuest.Character.Attributes;
using CoreAttributeSO = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    [CreateAssetMenu(fileName = "Elemental", menuName = "Gameplay/Elemental")]
    public class Elemental : ScriptableObject
    {
        [Serializable]
        public struct Multiplier
        {
            public ElementalAttribute Attribute;
            public float Value;
        }

        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: SerializeField] public CoreAttributeSO AttackAttribute { get; private set; }
        [field: SerializeField] public CoreAttributeSO ResistanceAttribute { get; private set; }

        [field: SerializeField] public Multiplier[] Multipliers { get; private set; }
    }
}