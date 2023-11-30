using System.Collections.Generic;
using CryptoQuest.Character.Beast;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public class BeastInventorySO : ScriptableObject
    {
        [field: SerializeField] public List<Beast> OwnedBeasts { get; set; } = new();
    }
}