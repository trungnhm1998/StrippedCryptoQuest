using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Item.Equipment;
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

        public void SetResultFail(IEquipment item)
        {
            EvolveResultUI.UpdateUIFail();
            EquipmentDetailUI.SetEquipment(item);
            EquipmentDetailUI.gameObject.SetActive(true);
        }

        public void SetResultSuccess(IEquipment item)
        {
            EvolveResultUI.UpdateUISuccess();
            EquipmentDetailUI.SetEquipment(item);
            EquipmentDetailUI.gameObject.SetActive(true);
        }
    }
}