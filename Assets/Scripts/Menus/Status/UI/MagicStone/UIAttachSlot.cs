using System;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Menu;
using CryptoQuest.UI.Tooltips.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class UIAttachSlot : MonoBehaviour
    {
        public event UnityAction Pressed;
        [SerializeField] private GameObject _selectingEffect;
        [SerializeField] private GameObject _selectedEffect;
        [SerializeField] private UISingleStone _singleStone;
        [SerializeField] private ShowTooltipEvent _showTooltipEvent;
        public UISingleStone SingleStoneUI => _singleStone;

        public void OnPressed() => Pressed?.Invoke();
        public void Cache() => _selectedEffect.SetActive(true);
        public void UnCache() => _selectedEffect.SetActive(false);

        public void Attach(IMagicStone stoneData)
        {
            _singleStone.SetInfo(stoneData);
            _singleStone.gameObject.SetActive(true);
            if (EventSystem.current.currentSelectedGameObject == gameObject)
                _showTooltipEvent.RaiseEvent(true);
        }

        public void Detach()
        {
            _singleStone.gameObject.SetActive(false);
        }

        public void Select() => _selectingEffect.SetActive(true);
        public void Deselect() => _selectingEffect.SetActive(false);
    }
}