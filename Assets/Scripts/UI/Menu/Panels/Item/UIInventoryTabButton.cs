using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIInventoryTabButton : MonoBehaviour
    {
        public event UnityAction<EConsumeable> Clicked;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private EConsumeable _consumableType;
        [SerializeField] private MultiInputButton _button;
        public EConsumeable ConsumableType => _consumableType;

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