using System.Collections.Generic;
using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using UnityEngine;
using ESlotType =
    CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquippingSlotContainer.EType;

namespace CryptoQuest
{
    public class CharacterEquipmentBehaviour : MonoBehaviour
    {
        [field: SerializeField] public HeroDataSO HeroData { get; private set; }

        [field: Header("Equipment slots"), SerializeField]
        public InventorySO Inventory { get; private set; }

        [field: SerializeField]
        public List<EquippingSlotContainer> Equipments { get; private set; } = new();

        [Header("Listening events")]
        [SerializeField] private EquipmentEventChannelSO _onEquipItem;

        [SerializeField] private EquipmentEventChannelSO _onUnequipItem;


        private void OnEnable()
        {
            _onEquipItem.EventRaised += EquipItem;
            _onUnequipItem.EventRaised += UnequipItem;
        }

        private void OnDisable()
        {
            _onEquipItem.EventRaised -= EquipItem;
            _onUnequipItem.EventRaised -= UnequipItem;
        }

        private void Awake()
        {
            InitEquipments();
        }

        public void InitEquipments()
        {
            InitEquipments(HeroData.Equipments);
        }

        public void InitEquipments(CharacterEquipments equipments)
        {
            if (equipments == null) return;
            Equipments.Clear();
            equipments.Inventory = Inventory;

            Equipments = equipments.GetEquippingSlots();
        }

        private void EquipItem(ESlotType type, EquipmentInfo equipment)
        {
            HeroData.Equipments.Equip(type, equipment);
        }

        private void UnequipItem(ESlotType type, EquipmentInfo equipment)
        {
            HeroData.Equipments.Unequip(type, equipment);
        }
    }
}