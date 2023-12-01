using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EquipmentDetailPresenter : MonoBehaviour
    {
        [field: SerializeField] public UIEquipmentDetail EquipmentDetailUI { get; private set; }

        [SerializeField] private EvolveableEquipmentsPresenter _evolveableEquipmentsPresenter;

        private void OnEnable()
        {
            _evolveableEquipmentsPresenter.OnInspectingEquipmentItemChange += HandleInspectingEquipmentItemChange;
        }

        private void OnDisable()
        {
            _evolveableEquipmentsPresenter.OnInspectingEquipmentItemChange -= HandleInspectingEquipmentItemChange;
        }

        private void HandleInspectingEquipmentItemChange(UIEquipmentItem item)
        {
            EquipmentDetailUI.SetEquipmentDetail(item.EquipmentData);
        }
    }
}
