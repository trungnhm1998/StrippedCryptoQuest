using CryptoQuest.BlackSmith.Interface;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EvolveResultPresenter : MonoBehaviour
    {
        [field: SerializeField] public UIResultPanel EvolveResultUI { get; private set; }

        [field: SerializeField] public UIEvolveEquipmentTooltip EquipmentDetailUI { get; private set; }

        private void OnEnable()
        {
            EvolveResultUI.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            EvolveResultUI.gameObject.SetActive(false);
            EquipmentDetailUI.gameObject.SetActive(false);
        }

        public void SetResultFail(IEvolvableEquipment item)
        {
            EvolveResultUI.UpdateUIFail();
            EquipmentDetailUI.SetEquipment(item.Equipment);
            EquipmentDetailUI.gameObject.SetActive(true);
        }

        public void SetResultSuccess(IEvolvableEquipment item)
        {
            EvolveResultUI.UpdateUISuccess();
            EquipmentDetailUI.SetEquipment(item.Equipment);
            EquipmentDetailUI.SetPreviewLevel(item.MinLevel, item.MaxLevel);
            EquipmentDetailUI.gameObject.SetActive(true);
        }
    }
}