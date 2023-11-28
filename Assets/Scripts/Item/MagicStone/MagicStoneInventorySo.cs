using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    public class MagicStoneInventorySo : ScriptableObject
    {
        [field: SerializeField] public List<MagicStoneInfo> MagicStones { get; set; } = new();
    }
}