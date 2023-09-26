using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public class UIEquipmentSection : UITransferSection
    {
        public static event UnityAction InspectItemEvent;

        [SerializeField] private UnityEvent EnterTransferSectionEvent;
        [SerializeField] private UnityEvent ResetTransferEvent;

        public override void EnterTransferSection()
        {
            base.EnterTransferSection();
            EnterTransferSectionEvent.Invoke();
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
    }
}
