using CryptoQuest.Input;
using CryptoQuest.Menus.Status.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class MagicStoneTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private InputMediatorSO _input;
        [SerializeField] private ShowMagicStoneEvent _showMagicStone;
        [SerializeField] private Transform _equipSlot;

        private bool _isEquipmentAvailable = false;

        public void OnSelect(BaseEventData _)
        {
            _input.MenuInteractEvent += RequestShowMagicStoneMenu;
            _isEquipmentAvailable = _equipSlot.GetChild(0).gameObject.activeSelf;
            Debug.Log($"<color=green>MagicStoneTrigger::OnSelect</color>");
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _input.MenuInteractEvent -= RequestShowMagicStoneMenu;
            Debug.Log($"<color=green>MagicStoneTrigger::OnDeselect</color>");
        }

        private void RequestShowMagicStoneMenu()
        {
            Debug.Log($"<color=green>MagicStoneTrigger::RequestShowMagicStoneMenu</color>");
            _showMagicStone.RaiseEvent(_isEquipmentAvailable);
        }
    }
}