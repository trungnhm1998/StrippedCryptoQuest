using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Menu;
using PolyAndCode.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UIEquipmentItem : MultiInputButton, ICell
    {
        public static event UnityAction<Button> InspectingRow;
        public event UnityAction<int> SelectedEvent;

        [Header("Game Components")]
        [SerializeField] private TMP_Text _itemName;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private GameObject _selectEffect;

        private GameObject _unEquipSlot;
        private int _index;

        public void Init(EquipmentInfo data, int index = 0)
        {
            _index = index;

            if (_unEquipSlot != null)
                _unEquipSlot.SetActive(false);

            _itemName.text = data.Item.name;
            if (data.Item != null && data.Item.DisplayName != null)
            {
                _name.StringReference = data.Item.DisplayName;
            }
        }

        public void OnPressed()
        {
            Debug.Log($"Inventory item pressed");
        }

        public override void OnSelect(BaseEventData eventData)
        {
            InspectingRow?.Invoke(this);
            SelectedEvent?.Invoke(_index);

            base.OnSelect(eventData);
            _selectEffect.SetActive(true);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            _selectEffect.SetActive(false);
        }
    }
}