using CryptoQuest.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Menus.Item.UI
{
    public class UIInventoryTabButton : MonoBehaviour
    {
        public event UnityAction<EConsumableType> Clicked;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private EConsumableType _consumableType;
        [SerializeField] private MultiInputButton _button;
        public EConsumableType ConsumableType => _consumableType;

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
            Highlight();
            Clicked?.Invoke(_consumableType);
        }

        public void UnHighlight()
        {
            _selectedBackground.SetActive(false);
        }

        public void Highlight()
        {
            _selectedBackground.SetActive(true);
        }
    }
}