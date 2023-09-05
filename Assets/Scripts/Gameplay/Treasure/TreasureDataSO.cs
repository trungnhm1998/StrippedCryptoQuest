using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.Items;
using UnityEngine;

namespace CryptoQuest.Gameplay.Treasure
{
    [CreateAssetMenu(fileName = "TreasureDataSO", menuName = "Gameplay/Treasure/Treasure Data")]
    public class TreasureDataSO : ScriptableObject
    {
        [SerializeReference] public List<ItemInfo> LootInfos = new();
    }
}