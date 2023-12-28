using CryptoQuest.Item.Consumable;
using CryptoQuest.UI.Extensions;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem
{
    public class UIConsumable : MonoBehaviour
    {
        [SerializeField] private Image _imgIcon;
        [SerializeField] private LocalizeStringEvent _txtName;
        [SerializeField] private Text _txtQuantity;
        [SerializeField] private string _quantityFormant = "x{0}";
        private ConsumableInfo _consumable;

        public void Init(ConsumableInfo consumable)
        {
            UnRegisterEventIfNeeded();
            _consumable = consumable;
            _imgIcon.LoadSpriteAndSet(consumable.Icon);
            _txtName.StringReference = consumable.DisplayName;
            consumable.QuantityChanged += UpdateQuantityText;
            UpdateQuantityText(consumable);
        }

        private void UnRegisterEventIfNeeded()
        {
            if (_consumable != null)
                _consumable.QuantityChanged -= UpdateQuantityText;
        }

        private void UpdateQuantityText(ConsumableInfo item)
        {
            _txtQuantity.text = string.Format(_quantityFormant, item.Quantity);
        }

        private void OnDisable()
        {
            UnRegisterEventIfNeeded();
        }
    }
}