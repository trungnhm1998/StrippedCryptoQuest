using System.Collections;
using CryptoQuest.Actions;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Sagas.Equipment
{
    public class UpdateHeroesNftEquipments : SagaBase<InventoryFilled>
    {
        [SerializeField] private AssetReferenceT<EquipmentInventory> _inventoryAsset;
        [SerializeField] private AssetReferenceT<PartySO> _partyAsset;
        private EquipmentInventory _equipmentInventory;
        private PartySO _partySO;
        private IPartyController _partyController;

        protected override void HandleAction(InventoryFilled _) => StartCoroutine(CoUpdateInventory());

        private IEnumerator CoUpdateInventory()
        {
            yield return CoLoad();

            foreach (var slot in _partySO.GetParty())
            {
                if (!slot.IsValid()) continue;
                // RemoveNftEquipmentsInInventoryIfHeroEquipping(slot.EquippingItems);
            }

            _partyController ??= ServiceProvider.GetService<IPartyController>();
            if (_partyController == null) yield break;
            foreach (var partySlot in _partyController.Slots)
            {
                if (!partySlot.IsValid()) continue;
                partySlot.HeroBehaviour.GetComponent<EquipmentsController>().Init();
            }
        }

        private IEnumerator CoLoad()
        {
            var inventoryHandle = _inventoryAsset.LoadAssetAsync();
            yield return inventoryHandle;
            _equipmentInventory = inventoryHandle.Result;

            var partyHandle = _partyAsset.LoadAssetAsync();
            yield return partyHandle;
            _partySO = partyHandle.Result;
        }

        private bool RemoveNftEquipmentsInInventoryIfHeroEquipping(Equipments equippingItems)
        {
            bool needUpdate = false;
            foreach (var equipmentSlot in equippingItems.Slots)
            {
                var equippingItem = equipmentSlot.Equipment;
                if (!equippingItem.IsValid()) continue;
                needUpdate = RemoveNftEquipmentFromInventoryIfHeroEquippingTheSame(equippingItem, equipmentSlot);
            }

            return needUpdate;
        }

        /// <summary>
        /// Compare using equipment <see cref="ItemInfo.Id"/>
        /// </summary>
        /// <param name="equippingItem"></param>
        /// <param name="equipmentSlot"></param>
        private bool RemoveNftEquipmentFromInventoryIfHeroEquippingTheSame(IEquipment equippingItem,
            EquipmentSlot equipmentSlot)
        {
            bool needUpdate = false;
            // for (var index = 0; index < _inventory.NftEquipments.Count; index++)
            // {
            //     var nftEquipment = _inventory.NftEquipments[index];
            //     if (nftEquipment != equippingItem) continue;
            //     _inventory.NftEquipments.RemoveAt(index);
            //     equipmentSlot.Equipment = nftEquipment;
            //     needUpdate = true;
            // }

            return needUpdate;
        }
    }
}