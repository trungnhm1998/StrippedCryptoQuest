using CryptoQuest.Menu;
using CryptoQuest.UI.Menu.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public class UIHeaderButton : MultiInputButton
    {
        public event UnityAction<MenuTypeSO> Pressed;
        public event UnityAction<UIHeaderButton> Selected;
        [SerializeField] private MenuTypeSO _typeSO;
        public MenuTypeSO TypeSO => _typeSO;
        [SerializeField] private Image _pointer;
        public Image Pointer => _pointer;
        [SerializeField] private TMP_Text _header;
        [SerializeField] private Color _normal;
        [SerializeField] private Color _disabled;

        public void OnPressed()
        {
            Pressed?.Invoke(_typeSO);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            Selected?.Invoke(this);
            if (!interactable)
            {
                return;
            }

            _pointer.enabled = true;
            base.OnSelect(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            _pointer.enabled = false;
            base.OnDeselect(eventData);
        }

        public void Focus()
        {
            _header.color = _normal;
        }

        public void UnFocus()
        {
            _header.color = _disabled;
        }
    }
}