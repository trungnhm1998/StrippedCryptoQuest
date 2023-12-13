using CryptoQuest.Ranch.Evolve.Interface;
using TMPro;
using UnityEngine;

namespace CryptoQuest.Ranch.Evolve.UI
{
    public class UIBeastEvolveInfoDetail : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _rate;
        [SerializeField] private TextMeshProUGUI _gold;
        [SerializeField] private TextMeshProUGUI _metad;

        public void SetConfirmInfo(IBeastEvolvableInfo infoData)
        {
            SetRate(infoData.Rate);
            SetGold(infoData.Gold);
            SetMetad((int)infoData.Metad);
        }

        private void SetRate(float rate) => _rate.text = $"{rate}%";
        private void SetGold(int gold) => _gold.text = $"{gold}G";
        private void SetMetad(int metad) => _metad.text = $"{metad}METAD";
    }
}
