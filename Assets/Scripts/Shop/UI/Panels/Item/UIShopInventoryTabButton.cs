using CryptoQuest.Menu;
using CryptoQuest.Shop.UI.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Shop.UI.Panels.Item
{
    public class UIShopInventoryTabButton : MonoBehaviour
    {
        public event UnityAction<ShopStateSO> Clicked;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private ShopStateSO _consumableType;
        [SerializeField] private MultiInputButton _button;
        public ShopStateSO ConsumableType => _consumableType;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            SetHighlight(true);
            Clicked?.Invoke(_consumableType);
        }

        public void SetHighlight(bool isEnable)
        {
            _selectedBackground.SetActive(isEnable);
        }
    }
}
