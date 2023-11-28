using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Gameplay.Inventory;

namespace CryptoQuest.Item.MagicStone
{
    [Serializable]
    public class MagicStoneInfo : ItemInfo<MagicStone>
    {
        public MagicStoneData StoneData { get; set; }
        public MagicStone MagicStonePrefab => Data;
        public int StoneLevel => StoneData.StoneLevel;
        public string AfterUpgradeStoneId => StoneData.AfterUpgradeStoneId;
        public PassiveAbility[] Passives => StoneData.Passives;
        public override bool AddToInventory(IInventoryController inventory) => true;
        public override bool RemoveFromInventory(IInventoryController inventory) => true;

        public bool AddToStoneInventory(IStoneInventoryController stoneInventoryController)
            => stoneInventoryController.Add(this);

        public bool RemoveFromStoneInventory(IStoneInventoryController stoneInventoryController)
            => stoneInventoryController.Remove(this);
    }
}