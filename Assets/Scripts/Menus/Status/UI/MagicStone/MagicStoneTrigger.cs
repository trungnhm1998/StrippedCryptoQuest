using CryptoQuest.Input;
using CryptoQuest.Menus.Status.Events;
using CryptoQuest.Menus.Status.UI.Equipment;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class MagicStoneTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private InputMediatorSO _input;
        [SerializeField] private ShowMagicStoneEvent _showMagicStone;
        [SerializeField] private UIEquipment _uiEquipment;

        public void OnSelect(BaseEventData _)
        {
            _input.MenuInteractEvent += RequestShowMagicStoneMenu;
            Debug.Log($"<color=green>MagicStoneTrigger::OnSelect</color>");
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _input.MenuInteractEvent -= RequestShowMagicStoneMenu;
            Debug.Log($"<color=green>MagicStoneTrigger::OnDeselect</color>");
        }

        private void OnDisable()
        {
            _input.MenuInteractEvent -= RequestShowMagicStoneMenu;
        }

        private void RequestShowMagicStoneMenu()
        {
            if (_uiEquipment.Equipment == null || _uiEquipment.Equipment.IsValid() == false) return;
            Debug.Log($"<color=green>MagicStoneTrigger::RequestShowMagicStoneMenu</color>");
            _showMagicStone.RaiseEvent(_uiEquipment.Equipment);
        }
    }
}