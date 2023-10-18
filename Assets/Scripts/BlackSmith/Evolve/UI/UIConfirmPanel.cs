using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using TMPro;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIConfirmPanel : MonoBehaviour
    {
        [SerializeField] private GameObject[] _beforeEvolveStars = new GameObject[5];
        [SerializeField] private GameObject[] _afterEvolveStars = new GameObject[5];
        [SerializeField] private TMP_Text _beforeEvolveLevel;
        [SerializeField] private TMP_Text _rate;
        [SerializeField] private TMP_Text _gold;
        [SerializeField] private TMP_Text _metad;

        public void SetConfirmInfo(IEvolvableData equipmentData)
        {
            SetStarBeforeEvolve(equipmentData.Stars);
            SetStarAfterEvolve(equipmentData.Stars + 1);
            SetLevelBeforeEvolve(equipmentData.Level);
            SetRate(equipmentData.Rate);
            SetGold(equipmentData.Gold);
            SetMetad((int)equipmentData.Metad);
        }

        private void SetStarBeforeEvolve(int star)
        {
            for (int i = 0; i < star; i++)
            {
                _beforeEvolveStars[i].SetActive(true);
            }
        }

        private void SetStarAfterEvolve(int star)
        {
            for (int i = 0; i < star; i++)
            {
                _afterEvolveStars[i].SetActive(true);
            }
        }

        private void SetLevelBeforeEvolve(int level) => _beforeEvolveLevel.text = $"Lv {level}";
        private void SetRate(int rate) => _rate.text = $"{rate}%";
        private void SetGold(int gold) => _gold.text = $"{gold}G";
        private void SetMetad(int metad) => _metad.text = $"{metad}METAD";

    }
}
