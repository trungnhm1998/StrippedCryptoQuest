using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Inventory
{
    public class UIInventoryTabButton : MonoBehaviour
    {
        public event UnityAction<UsableTypeSO> Clicked;

        [SerializeField] private UsableTypeSO _typeMenuItem;

        public void OnClicked()
        {
            Clicked?.Invoke(_typeMenuItem);
        }

        public void Select()
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}