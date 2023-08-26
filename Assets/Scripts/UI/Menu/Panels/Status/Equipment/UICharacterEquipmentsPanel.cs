using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.UI.Menu.MenuStates.StatusStates;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UICharacterEquipmentsPanel : MonoBehaviour
    {
        [Header("Game Components")]
        [SerializeField] private TooltipProvider _tooltipProvider;
        [SerializeField] private UIStatusMenu _mainPanel;
        [SerializeField] private UICharacterEquipmentSlot[] _equipmentSlots;
        [SerializeField] private GameObject _equipmentSlotParent;
        [SerializeField] private UIEquipmentSlotButton _defaultSelection;
        [SerializeField] private RectTransform _tooltipSafeArea;

        private Dictionary<EquipmentSlot.EType, UICharacterEquipmentSlot> _equipmentSlotsCache = null;

        private Dictionary<EquipmentSlot.EType, UICharacterEquipmentSlot> EquipmentSlots
        {
            get
            {
                if (_equipmentSlotsCache == null)
                {
                    _equipmentSlotsCache = new Dictionary<EquipmentSlot.EType, UICharacterEquipmentSlot>();
                    foreach (var equipmentSlot in _equipmentSlots)
                    {
                        _equipmentSlotsCache.Add(equipmentSlot.SlotType, equipmentSlot);
                    }
                }

                return _equipmentSlotsCache;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _equipmentSlots = GetComponentsInChildren<UICharacterEquipmentSlot>();
        }
#endif

        private void Awake()
        {
            _tooltipProvider.Tooltip.SetSafeArea(_tooltipSafeArea);
            foreach (var equipmentSlot in _equipmentSlots)
            {
                equipmentSlot.ChangingEquipment += ChangingEquipment;
                equipmentSlot.Tooltip = _tooltipProvider.Tooltip;
            }
        }

        private void OnDestroy()
        {
            foreach (var equipmentSlot in _equipmentSlots)
            {
                equipmentSlot.ChangingEquipment -= ChangingEquipment;
            }
        }

        private void ChangingEquipment(EquipmentSlot.EType slotType)
        {
            _mainPanel.State.RequestStateChange(StatusMenuStateMachine.EquipmentSelection);
        }

        public void SetEquipment(CharacterEquipments characterEquipments)
        {
            ResetEquipments();
            for (int i = 0; i < characterEquipments.Slots.Count; i++)
            {
                var equipmentSlot = characterEquipments.Slots[i];
                if (equipmentSlot.IsValid() == false) continue;
                var uiEquipmentSlot = EquipmentSlots[equipmentSlot.Type];
                uiEquipmentSlot.Init(equipmentSlot.Equipment);
            }
        }

        private void ResetEquipments()
        {
            foreach (var equipmentSlot in _equipmentSlots)
            {
                equipmentSlot.Reset();
            }
        }

        #region State context

        public void Init()
        {
            Show();
            _defaultSelection.Select();
        }

        public void Show(bool isShown = true)
        {
            _equipmentSlotParent.SetActive(isShown);
        }

        #endregion
    }
}