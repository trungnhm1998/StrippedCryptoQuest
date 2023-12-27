using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    public class MagicStoneInventory : ScriptableObject
    {
        [field: SerializeReference, SubclassSelector]
        public List<IMagicStone> MagicStones { get; set; } = new();

        public IMagicStone GetStone(int id)
        {
            foreach (var stone in MagicStones)
            {
                if (stone.IsValid() && stone.ID == id)
                    return stone;
            }
            return NullMagicStone.Instance;
        }
    }
}