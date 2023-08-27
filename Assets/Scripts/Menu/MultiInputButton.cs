using System;
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
        public event Action Selected;
        public event Action DeSelected;

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

        public override void OnSelect(BaseEventData eventData)
        {
            if (!enabled) return;
            Selected?.Invoke();
            Handler.UpdateSelection(gameObject);
            base.OnSelect(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            if (!enabled) return;
            DeSelected?.Invoke();
            base.OnDeselect(eventData);
        }
    }
}