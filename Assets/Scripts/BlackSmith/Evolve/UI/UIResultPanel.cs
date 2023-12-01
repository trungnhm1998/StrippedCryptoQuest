using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIResultPanel : MonoBehaviour
    {
        [SerializeField] private UIEquipmentDetail _uiEquipmentDetail;
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private MultiInputButton _defaultButton;
        [SerializeField] private LocalizedString _successMessage;
        [SerializeField] private LocalizedString _failMessage;

        private void OnEnable()
        {
            _defaultButton.Select();
        }

        public void SetSuccessInfo(IEvolvableEquipment evolvedEquipmentData)
        {
            _resultText.text = _successMessage.GetLocalizedString();
            if (evolvedEquipmentData != null)
                _uiEquipmentDetail.SetEquipmentDetail(evolvedEquipmentData);
        }

        public void SetFailInfo(IEvolvableEquipment equipmentData)
        {
            _resultText.text = _failMessage.GetLocalizedString();
            if (equipmentData != null)
                _uiEquipmentDetail.SetEquipmentDetail(equipmentData);
        }
    }
}
