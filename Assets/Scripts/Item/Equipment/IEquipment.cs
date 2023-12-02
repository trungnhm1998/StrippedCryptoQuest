using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Gameplay.Inventory;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine.Localization;

namespace CryptoQuest.Item.Equipment
{
    public interface IEquipment
    {
        public uint Id { get; set; }
        public int Level { get; set; }
        public EquipmentData Data { get; set; }
        public AttributeWithValue[] Stats { get; }
        public RaritySO Rarity { get; }
        public float ValuePerLvl { get; }
        public EquipmentPrefab Prefab { get; }
        public EquipmentTypeSO Type { get; }
        public ESlot[] RequiredSlots { get; }
        public ESlot[] AllowedSlots { get; }

        public PassiveAbility[] Passives { get; }

        public bool IsNft { get; }
        LocalizedString DisplayName { get; }

        public bool IsValid();
        public bool AddToInventory(IInventoryController inventory);
        public bool RemoveFromInventory(IInventoryController inventory);
    }
}