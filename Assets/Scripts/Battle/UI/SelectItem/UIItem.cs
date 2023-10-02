using System;
using CryptoQuest.Item;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Pool;

namespace CryptoQuest.Battle.UI.SelectItem
{
    public class UIItem : MonoBehaviour
    {
        public Action<UIItem> Selected;
        
        [SerializeField] private TMP_Text _quantity;
        [SerializeField] private LocalizeStringEvent _nameStringEvent;

        private ConsumableInfo _item;
        public ConsumableInfo Item => _item;

        public void Init(ConsumableInfo item)
        {
            _item = item;
            _nameStringEvent.StringReference = item.Data.DisplayName;
            SetQuantityText(item);
        }

        private void SetQuantityText(ConsumableInfo item)
        {
            if (Item == null || Item.Data != item.Data) return;
            _quantity.text = $"x{item.Quantity}";
        }

        private void OnEnable()
        {
            ConsumableInfo.QuantityReduced += SetQuantityText;
        }

        public void OnPressed()
        {
            Selected?.Invoke(this);
        }

        private void OnDestroy()
        {
            ConsumableInfo.QuantityReduced -= SetQuantityText;
        }
    }
}