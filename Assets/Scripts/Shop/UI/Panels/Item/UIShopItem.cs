using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using CryptoQuest.Shop.UI.ScriptableObjects;
using CryptoQuest.UI.Menu.Panels.Status;
using PolyAndCode.UI;
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
        public Action<IShopItemData> OnSubmit;

        [SerializeField] private MultiInputButton _button;
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _nameText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private GameObject _gemInfo;
        private const string CURRENCY = "G";

        private IShopItemData _shopItemData;

        private void OnEnable()
        {
            if (_shopItemData == null || _shopItemData.Icon == null)
            {
                return;
            }

            StartCoroutine(LoadSpriteAndSet(_shopItemData.Icon));
        }

        public void Init(IShopItemData shopItemData, bool isBuy)
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
        }

        private IEnumerator LoadSpriteAndSet(AssetReferenceT<Sprite> equipmentTypeIcon)
        {
            if (equipmentTypeIcon.RuntimeKeyIsValid() == false) yield break;
            var handle = equipmentTypeIcon.LoadAssetAsync<Sprite>();
            yield return handle;

            _icon.sprite = handle.Result;
        }
    }

    public interface IShopItemData
    {
        public ItemInfo Item { get; }
        public AssetReferenceT<Sprite> Icon { get; }
        public LocalizedString DisplayName { get; }
        public int Price { get; }
        public int SellPrice { get; }
        public bool HasGem { get; }
    }

    public class EquipmentItem : IShopItemData
    {
        public ItemInfo Item => _equipment;
        public AssetReferenceT<Sprite> Icon => _equipment.Data.Image;
        public LocalizedString DisplayName => _equipment.Data.DisplayName;
        public int Price => _equipment.Price;
        public int SellPrice => _equipment.SellPrice;
        public bool HasGem => true;

        private EquipmentInfo _equipment;

        public EquipmentItem(EquipmentInfo equipmentInfo)
        {
            _equipment = equipmentInfo;
        }
    }

    public class ConsumableItem : IShopItemData
    {
        public ItemInfo Item => _consumable;
        public AssetReferenceT<Sprite> Icon => _consumable.Data.Image;
        public LocalizedString DisplayName => _consumable.Data.DisplayName;
        public int Price => _consumable.Price;
        public int SellPrice => _consumable.SellPrice;
        public bool HasGem => false;

        private ConsumableInfo _consumable;

        public ConsumableItem(ConsumableInfo consumableInfo)
        {
            _consumable = consumableInfo;
        }
    }
}
