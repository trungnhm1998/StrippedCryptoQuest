using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public class UIEquipmentSection : UITransferSection
    {
        public static event UnityAction InspectItemEvent;
        public event UnityAction<bool> SendingPhaseEvent;

        [SerializeField] private UnityEvent EnterTransferSectionEvent;
        [SerializeField] private UnityEvent ExitTransferSectionEvent;
        [SerializeField] private UnityEvent ResetTransferEvent;
        [SerializeField] private UnityEvent<Vector2> _switchBoardEvent;

        public override void EnterTransferSection()
        {
            base.EnterTransferSection();
            EnterTransferSectionEvent.Invoke();
        }

        public override void ExitTransferSection()
        {
            base.ExitTransferSection();
            ExitTransferSectionEvent.Invoke();
        }

        public override void ResetTransfer()
        {
            base.ResetTransfer();
            ResetTransferEvent.Invoke();
        }

        public void OnInspectItem()
        {
            InspectItemEvent?.Invoke();
        }

        public void OnSwitchBoard(Vector2 direction)
        {
            _switchBoardEvent.Invoke(direction);
        }

        public override void SendItems()
        {
            base.SendItems();
            SendingPhaseEvent?.Invoke(true);
        }

        protected override void YesButtonPressed()
        {
            base.YesButtonPressed();
            SendingPhaseEvent?.Invoke(false);
        }

        protected override void NoButtonPressed()
        {
            base.NoButtonPressed();
            SendingPhaseEvent?.Invoke(false);
        }
    }
}
