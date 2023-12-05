using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    public class MagicStoneInventoryController : MonoBehaviour, IStoneInventoryController
    {
        [SerializeField] private MagicStoneInventory _stoneInventory;

        private void Awake()
        {
            ServiceProvider.Provide<IStoneInventoryController>(this);
        }

        public bool Add(IMagicStone magicStone)
        {
            if (magicStone == null || !magicStone.IsValid())
            {
                Debug.LogWarning($"Stone is null or invalid");
                return false;
            }

            _stoneInventory.MagicStones.Add(magicStone);
            return true;
        }

        public bool Remove(IMagicStone stone)
        {
            if (stone == null || !stone.IsValid())
            {
                return _stoneInventory.MagicStones.Remove(stone);
            }

            Debug.LogWarning($"Stone is null or invalid");
            return false;
        }
    }
}