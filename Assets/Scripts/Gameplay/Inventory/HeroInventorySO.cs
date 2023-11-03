using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public class HeroInventorySO : ScriptableObject
    {
        [field: SerializeField] public List<HeroSpec> OwnedHeroes { get; set; } = new();
    }
}