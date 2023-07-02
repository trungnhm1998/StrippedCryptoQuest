using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    [AddComponentMenu("UI/MultiInputButton")]
    public class MultiInputButton : Button
    {
        [ReadOnly] public bool IsSelected;

        [SerializeField] private MenuSelectionHandler _menuSelectionHandler;

        private new void Awake()
        {
            _menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            _menuSelectionHandler?.HandleMouseEnter(gameObject);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _menuSelectionHandler?.HandleMouseExit(gameObject);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            IsSelected = true;
            _menuSelectionHandler?.UpdateSelection(gameObject);
            base.OnSelect(eventData);
        }

        public void UpdateSelected()
        {
            _menuSelectionHandler?.UpdateSelection(gameObject);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            if (_menuSelectionHandler.AllowsSubmit())
                base.OnSubmit(eventData);
        }
    }
}