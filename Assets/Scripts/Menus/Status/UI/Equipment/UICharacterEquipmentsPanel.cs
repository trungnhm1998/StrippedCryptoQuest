using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    /// <summary>
    /// Show currently equipped equipments
    /// </summary>
    public class UICharacterEquipmentsPanel : MonoBehaviour
    {
        // refs
        [SerializeField] private UICharacterEquipmentSlot[] _equipmentSlots;

        // config
        [SerializeField] private GameObject _equipmentSlotParent;

        private Dictionary<ESlot, UICharacterEquipmentSlot> _equipmentSlotsCache = new();

        private Dictionary<ESlot, UICharacterEquipmentSlot> EquipmentSlots
        {
            get
            {
                if (_equipmentSlotsCache != null && _equipmentSlotsCache.Count != 0) return _equipmentSlotsCache;
                _equipmentSlotsCache = new Dictionary<ESlot, UICharacterEquipmentSlot>();
                foreach (var equipmentSlot in _equipmentSlots)
                    _equipmentSlotsCache.Add(equipmentSlot.SlotType, equipmentSlot);

                return _equipmentSlotsCache;
            }
        }

#if UNITY_EDITOR
        private void OnValidate() => _equipmentSlots = GetComponentsInChildren<UICharacterEquipmentSlot>();
#endif

        public void Show(HeroBehaviour hero)
        {
            if (hero == null || hero.IsValid() == false) return;
            _equipmentSlotParent.SetActive(true);

            RenderEquippingItems(hero);
        }

        public void Hide() => _equipmentSlotParent.SetActive(false);

        private void RenderEquippingItems(HeroBehaviour hero)
        {
            var equipments = hero.GetComponent<EquipmentsController>().Equipments;
            ResetEquipmentsUI();
            foreach (var equipmentSlot in equipments.Slots)
            {
                if (equipmentSlot.IsValid() == false) continue;
                var uiEquipmentSlot = EquipmentSlots[equipmentSlot.Type];
                uiEquipmentSlot.Init(equipmentSlot.Equipment);
            }
        }

        private void ResetEquipmentsUI()
        {
            foreach (var equipmentSlot in _equipmentSlots) equipmentSlot.Reset();
        }

        public void SetActiveAllEquipmentSlots(bool enable)
        {
            foreach (var slot in _equipmentSlots)
            {
                var button = slot.GetComponent<Button>();
                button.enabled = enable;
            }
        }
    }
}