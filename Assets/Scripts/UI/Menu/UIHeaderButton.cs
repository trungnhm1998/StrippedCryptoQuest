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
        [SerializeField] private MenuTypeSO _typeSO;
        public MenuTypeSO TypeSO => _typeSO;
        [SerializeField] private Image _pointer;
        public Image Pointer => _pointer;
        [SerializeField] private TMP_Text _header;
        [SerializeField] private Color _normal;
        [SerializeField] private Color _disabled;
        private bool _enabled = true;

        public void OnPressed()
        {
            if (!_enabled) return;
            Pressed?.Invoke(_typeSO);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            if (!_enabled) return;
            _pointer.enabled = true;
            base.OnSelect(eventData);
        }
        public override void OnDeselect(BaseEventData eventData)
        {
            if (!_enabled) return;

            _pointer.enabled = false;
            base.OnDeselect(eventData);
        }

        public void Disable()
        {
            _enabled = false;
            _header.color = _disabled;
        }

        public void Enable()
        {
            _enabled = true;
            _header.color = _normal;
        }
    }
}