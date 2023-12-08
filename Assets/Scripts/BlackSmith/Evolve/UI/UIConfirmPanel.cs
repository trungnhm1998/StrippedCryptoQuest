using CryptoQuest.BlackSmith.Interface;
using TMPro;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIConfirmPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _rate;
        [SerializeField] private TMP_Text _gold;
        [SerializeField] private TMP_Text _metad;

        public void SetConfirmInfo(IEvolvableEquipment equipmentData)
        {
            SetRate(equipmentData.Rate);
            SetGold(equipmentData.Gold);
            SetMetad((int)equipmentData.Metad);
        }

        private void SetRate(int rate) => _rate.text = $"{rate}%";
        private void SetGold(int gold) => _gold.text = $"{gold}G";
        private void SetMetad(int metad) => _metad.text = $"{metad}METAD";
    }
}
