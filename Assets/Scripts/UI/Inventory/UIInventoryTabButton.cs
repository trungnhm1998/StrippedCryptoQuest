using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Inventory
{
    public class UIInventoryTabButton : MonoBehaviour
    {
        public event UnityAction<EItemType> Clicked;

        [SerializeField] private EItemType _typeMenuItem;

        public void OnClicked()
        {
            Clicked?.Invoke(_typeMenuItem);
        }

        public void Select()
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}