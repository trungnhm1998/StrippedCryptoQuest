using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EquipmentsPresenter : MonoBehaviour
    {
        [SerializeField] private UIEvolvableEquipmentList _evolvableEquipmentsUI;

        [field: SerializeField] public BlackSmithDialogsPresenter DialogPresenter { get; private set; }

        [field: SerializeField, Header("Localization")]
        public LocalizedString SelectBaseMessage { get; private set; }

        [field: SerializeField] public LocalizedString SelectMaterialMessage { get; private set; }

        private IEvolvableModel _model;

        private void Awake()
        {
            _model = GetComponent<IEvolvableModel>();
        }

        public void Init()
        {
            var evolvableEquipments = _model.GetEvolableEquipments();
            _evolvableEquipmentsUI.RenderEquipments(evolvableEquipments);
        }
    }
}