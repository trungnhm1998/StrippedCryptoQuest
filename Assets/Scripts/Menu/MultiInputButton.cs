using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Menu
{
    /// <summary>
    /// An extension of Unity's base Button class, to support input from both mouse and keyboard/Joysticks
    /// </summary>
    [AddComponentMenu("IndiGamesCore/UI/MultiInputButton")]
    public class MultiInputButton : Button
    {
        [ReadOnly] public bool IsSelected;

        private MenuSelectionHandler _menuSelectionHandler;

        private new void Awake()
        {
            GetMenuSelectionHandler();
        }

        private MenuSelectionHandler GetMenuSelectionHandler()
        {
            if (_menuSelectionHandler == null)
                _menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
            return _menuSelectionHandler;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            GetMenuSelectionHandler().HandleMouseEnter(gameObject);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            GetMenuSelectionHandler().HandleMouseExit(gameObject);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            IsSelected = true;
            GetMenuSelectionHandler().UpdateSelection(gameObject);
            base.OnSelect(eventData);
        }

        public void UpdateSelected()
        {
            GetMenuSelectionHandler().UpdateSelection(gameObject);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            if (GetMenuSelectionHandler().AllowsSubmit())
                base.OnSubmit(eventData);
        }
    }
}