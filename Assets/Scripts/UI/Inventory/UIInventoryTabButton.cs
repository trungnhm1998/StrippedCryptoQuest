using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Inventory
{
    public class UIInventoryTabButton : MonoBehaviour
    {
        public event UnityAction<int> Clicked;

        [SerializeField] private int _tabTypeId;

        public void OnClicked()
        {
            Clicked?.Invoke(_tabTypeId);
        }

        public void Select()
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}