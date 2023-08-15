using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menu
{
    [AddComponentMenu("IndiGamesCore/UI/MultiInputSelectableElement")]
    public class MultiInputSelectableElement : MonoBehaviour, ISelectHandler
    {
        private MenuSelectionHandler _menuSelectionHandler;

        private void Awake()
        {
            _menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            _menuSelectionHandler.UpdateSelection(gameObject);
        }
    }
}