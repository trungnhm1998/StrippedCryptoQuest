using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EvolveSystem : MonoBehaviour
    {
        [field: SerializeField] public EquipmentsPresenter EquipmentsPresenter { get; private set; }
        [field: SerializeField] public EquipmentDetailPresenter EquipmentDetailPresenter { get; private set; }
        [field: SerializeField] public ConfirmEvolveDialog ConfirmEvolveDialog { get; private set; }
        [field: SerializeField] public UIEvolvableEquipmentList EvolvableEquipmentListUI { get; private set; }
        [field: SerializeField] public EvolveResultPresenter EvolveResultPresenter { get; private set; }

        [field: SerializeField] public LocalizedString SelectEquipmentToEvolveText { get; private set; }
        [field: SerializeField] public LocalizedString SelectMaterialText { get; private set; }
        [field: SerializeField] public LocalizedString ConfirmEvolveText { get; private set; }
    }
}
