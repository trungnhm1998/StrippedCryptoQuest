using CryptoQuest.Actions;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Sagas.Profile
{
    public class UpdateHeroesNftEquipments : SagaBase<InventoryFilled>
    {
        [SerializeField] private InventorySO _inventory;

        protected override void HandleAction(InventoryFilled ctx)
        {
            var partyController = ServiceProvider.GetService<IPartyController>();
            foreach (var slot in partyController.Slots)
            {
                if (!slot.IsValid()) continue;
                if (!RemoveNftEquipmentsInInventoryIfHeroEquipping(slot.HeroBehaviour)) continue;
                slot.HeroBehaviour.GetComponent<EquipmentsController>().Init();
            }
        }

        private bool RemoveNftEquipmentsInInventoryIfHeroEquipping(HeroBehaviour hero)
        {
            bool needUpdate = false;
            hero.TryGetComponent(out EquipmentsController equipmentsController);
            var equipments = equipmentsController.Equipments;
            foreach (var equipmentSlot in equipments.Slots)
            {
                var equippingItem = equipmentSlot.Equipment;
                if (!equippingItem.IsValid() || equippingItem.Id == 0) continue;
                needUpdate = RemoveNftEquipmentFromInventoryIfHeroEquippingTheSame(equippingItem, equipmentSlot);
            }

            return needUpdate;
        }

        /// <summary>
        /// Compare using equipment <see cref="ItemInfo.Id"/>
        /// </summary>
        /// <param name="equippingItem"></param>
        /// <param name="equipmentSlot"></param>
        private bool RemoveNftEquipmentFromInventoryIfHeroEquippingTheSame(EquipmentInfo equippingItem, EquipmentSlot equipmentSlot)
        {
            bool needUpdate = false;
            for (var index = 0; index < _inventory.NftEquipments.Count; index++)
            {
                var nftEquipment = _inventory.NftEquipments[index];
                if (nftEquipment.Id != equippingItem.Id) continue;
                _inventory.NftEquipments.RemoveAt(index);
                equipmentSlot.Equipment = nftEquipment;
                needUpdate = true;
            }

            return needUpdate;
        }
    }
}