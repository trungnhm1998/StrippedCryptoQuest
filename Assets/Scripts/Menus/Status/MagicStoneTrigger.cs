using CryptoQuest.Input;
using CryptoQuest.Menus.Status.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Status
{
    public class MagicStoneTrigger : MonoBehaviour, ISelectHandler
    {
        [SerializeField] private InputMediatorSO _input;
        [SerializeField] private ShowMagicStoneEvent _showMagicStone;
        [SerializeField] private Transform _equipSlot;

        private bool _isEquipmentAvailable = false;

        private void Awake()
        {
            _input.MenuInteractEvent += RequestShowMagicStoneMenu;
        }

        private void OnDisable()
        {
            _input.MenuInteractEvent -= RequestShowMagicStoneMenu;
        }

        public void OnSelect(BaseEventData _)
        {
            _isEquipmentAvailable = _equipSlot.GetChild(0).gameObject.activeSelf;
        }

        private void RequestShowMagicStoneMenu()
        {
            if (_isEquipmentAvailable) _showMagicStone.RaiseEvent(true);
        }
    }
}