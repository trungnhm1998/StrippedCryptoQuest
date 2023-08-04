using System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public class UsableInformation : ItemInformation
    {
        [field: SerializeField] public int Quantity { get; private set; }
    }
}