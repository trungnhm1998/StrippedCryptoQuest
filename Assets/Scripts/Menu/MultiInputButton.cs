using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Menu
{
    /// <summary>
    /// An extension of Unity's base Button class, to support input from
    /// both mouse and keyboard/Joysticks
    /// </summary>
    [AddComponentMenu("IndiGamesCore/UI/MultiInputButton")]
    public class MultiInputButton : Button
    {
        [ReadOnly] public bool IsSelected;

        private MenuSelectionHandler _menuSelectionHandler;

        /// <summary>
        /// Lazy load the MenuSelectionHandler
        /// </summary>
        private MenuSelectionHandler Handler
        {
            get
            {
                if (_menuSelectionHandler == null)
                    _menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
                return _menuSelectionHandler;
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!Handler.HasCursorMoved) return;
            if (!enabled) return;
            Handler.HandleMouseEnter(gameObject);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (!Handler.HasCursorMoved) return;
            if (!enabled) return;
            Handler.HandleMouseExit(gameObject);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            if (!enabled) return;
            IsSelected = true;
            Handler.UpdateSelection(gameObject);
            base.OnSelect(eventData);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            if (Handler.AllowsSubmit())
                base.OnSubmit(eventData);
        }
    }
}