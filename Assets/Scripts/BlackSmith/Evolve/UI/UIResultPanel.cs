using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using TMPro;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIResultPanel : MonoBehaviour
    {
        [SerializeField] private UIEquipmentDetail _uiEquipmentDetail;
        [SerializeField] private TMP_Text _resultText;

        public void SetSuccessInfo(IEvolvableData evolvedEquipmentData)
        {
            _resultText.text = "SUCCESS!!";
            _uiEquipmentDetail.SetEquipmentDetail(evolvedEquipmentData);
        }

        public void SetFailInfo(IEvolvableData equipmentData)
        {
            _resultText.text = "FAILURE!!";
            _uiEquipmentDetail.SetEquipmentDetail(equipmentData);
        }
    }
}
