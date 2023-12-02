using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    public class MagicStoneInventorySo : ScriptableObject
    {
        [field: SerializeReference, SubclassSelector] public List<IMagicStone> MagicStones { get; set; } = new();
    }
}