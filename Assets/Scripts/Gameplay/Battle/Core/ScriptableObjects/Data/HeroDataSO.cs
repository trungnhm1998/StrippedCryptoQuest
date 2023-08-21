using CryptoQuest.Config;
using UnityEngine;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "HeroDataSO", menuName = "Gameplay/Character/Hero Data")]
    public class HeroDataSO : CharacterDataSO
    {
        [field: SerializeField] public Sprite BattleIconSprite { get; private set; }
        [field: SerializeField] public Sprite Avatar { get; private set; }
        [field: SerializeField] public Sprite ElementIcon { get; private set; }
        [field: SerializeField] public int Level { get; private set; }

        [field: SerializeField, Header("Equipment slots")]
        public InventoryConfigSO InventoryConfig { get; private set; }

        [field: SerializeField, NonReorderable]
        public CharacterEquipments Equipments { get; private set; } = new();

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (InventoryConfig == null) return;
            Equipments.Initialize(InventoryConfig.CategorySlotIndex, InventoryConfig.SlotTypeIndex);
        }
#endif
    }
}