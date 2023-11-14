using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    public class OpenedChestsSO : ScriptableObject
    {
        public event Action Changed;
        [field: SerializeField] public List<string> Chests { get; set; } = new();

        public void AddChest(string chest)
        {
            if (Chests.Contains(chest)) return;
            Chests.Add(chest);
            Changed?.Invoke();
        }

        public bool Contains(string chestGuid) => Chests.Contains(chestGuid);
    }
}