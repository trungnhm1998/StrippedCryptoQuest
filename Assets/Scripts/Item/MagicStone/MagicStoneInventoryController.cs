using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    public class MagicStoneInventoryController : MonoBehaviour, IStoneInventoryController
    {
        [SerializeField] private MagicStoneInventorySo _stoneInventory;

        private void Awake()
        {
            ServiceProvider.Provide<IStoneInventoryController>(this);
        }

        public bool Add(MagicStone stone)
        {
            if (stone != null && stone.IsValid())
            {
                Debug.LogWarning($"Stone is null or invalid");
                return false;
            }

            _stoneInventory.MagicStones.Add(stone);
            return true;
        }

        public bool Remove(MagicStone stone)
        {
            if (stone != null && stone.IsValid())
            {
                return _stoneInventory.MagicStones.Remove(stone);
            }

            Debug.LogWarning($"Stone is null or invalid");
            return false;
        }
    }
}