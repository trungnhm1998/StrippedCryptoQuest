using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using CryptoQuest.Shop.UI.Item;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Shop.UI.Panels.Item
{
    public class UIShopItem : MonoBehaviour
    {
        public Action<IShopItem> OnSubmit;
        public Action<IShopItem> OnSelected;

        [SerializeField] private MultiInputButton _button;
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _nameText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private GameObject _gemInfo;
        private const string CURRENCY = "G";

        private IShopItem _shopItemData;

        private void OnEnable()
        {
            _button.Selected += OnSelect;

            if (_shopItemData == null || _shopItemData.Icon == null)
            {
                return;
            }

            StartCoroutine(LoadSpriteAndSet(_shopItemData.Icon));
        }

        private void OnDisable()
        {
            _button.Selected -= OnSelect;
        }

        public void Init(IShopItem shopItemData, bool isBuy)
        {
            _shopItemData = shopItemData;
            _nameText.StringReference = _shopItemData.DisplayName;
            _priceText.text = $"{(isBuy ? _shopItemData.Price : _shopItemData.SellPrice)}{CURRENCY}";
            _gemInfo.SetActive(_shopItemData.HasGem);
        }

        public void Submit()
        {
            OnSubmit?.Invoke(_shopItemData);
        }

        public void Select()
        {
            _button.Select();
            OnSelected?.Invoke(_shopItemData);
        }

        private void OnSelect()
        {
            OnSelected?.Invoke(_shopItemData);
        }

        private IEnumerator LoadSpriteAndSet(AssetReferenceT<Sprite> equipmentTypeIcon)
        {
            if (equipmentTypeIcon.RuntimeKeyIsValid() == false) yield break;
            var handle = equipmentTypeIcon.LoadAssetAsync<Sprite>();
            yield return handle;

            _icon.sprite = handle.Result;
        }
    }
}
