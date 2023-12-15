using System;
using TMPro;
using UnityEngine;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class UIConfirmStoneUpgradePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _rate;
        [SerializeField] private TMP_Text _gold;
        [SerializeField] private TMP_Text _metad;

        public void SetConfirmInfo(UpgradableStoneData stoneData)
        {
            SetRate(stoneData.Probability);
            SetGold(stoneData.Gold);
            SetMetad((int)stoneData.Metad);
        }

        private void SetRate(int rate) => _rate.text = $"{rate}%";
        private void SetGold(float gold) => _gold.text = $"{gold}G";
        private void SetMetad(float metad) => _metad.text = $"{metad}METAD";
    }

    [Serializable]
    public struct UpgradableStoneData
    {
        public int ID;
        public int AfterUpgradeLevel;
        public int Probability;
        public float Gold;
        public float Metad;
    }
}