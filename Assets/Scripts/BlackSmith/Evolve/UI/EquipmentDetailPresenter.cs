using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EquipmentDetailPresenter : MonoBehaviour
    {
        [field: SerializeField] public UIEquipmentDetail EquipmentDetailUI { get; private set; }

        [SerializeField] private EquipmentsPresenter _equipmentsPresenter;

        private void OnEnable()
        {
            // _equipmentsPresenter.OnInspectingEquipmentItemChange += HandleInspectingEquipmentCacheableItemChange;
        }

        private void OnDisable()
        {
            // _equipmentsPresenter.OnInspectingEquipmentItemChange -= HandleInspectingEquipmentCacheableItemChange;
        }

        private void HandleInspectingEquipmentCacheableItemChange(UIEquipmentItem item)
        {
        }
    }
}
