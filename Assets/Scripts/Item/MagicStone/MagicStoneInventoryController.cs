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

        public bool Add(MagicStoneInfo stoneInfo)
        {
            if (stoneInfo == null || !stoneInfo.IsValid())
            {
                Debug.LogWarning($"Stone is null or invalid");
                return false;
            }

            _stoneInventory.MagicStones.Add(stoneInfo);
            return true;
        }

        public bool Remove(MagicStoneInfo stoneInfo)
        {
            if (stoneInfo != null && stoneInfo.IsValid())
            {
                return _stoneInventory.MagicStones.Remove(stoneInfo);
            }

            Debug.LogWarning($"Stone is null or invalid");
            return false;
        }
    }
}