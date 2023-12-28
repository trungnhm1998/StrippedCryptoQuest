using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Ranch.Upgrade.UI;
using UnityEngine;

namespace CryptoQuest.Ranch.Upgrade.Presenters
{
    public class UpgradePresenter : MonoBehaviour
    {
        [field: SerializeField] public UIConfigBeastUpgradePresenter ConfigBeast { get; private set; }
        [field: SerializeField] public UIResultUpgradeBeastList ResultBeast { get; private set; }
        [field: SerializeField] public UIBeastUpgradeDetail UiBeastUpgradeDetail { get; private set; }
        [field: SerializeField] public UIBeastUpgradeList BeastList { get; private set; }
        [field: SerializeField] public GameObject LeftPanel { get; private set; }

        public IBeast BeastToUpgrade { get; private set; } = NullBeast.Instance;

        public bool Interactable
        {
            get => BeastList.Interactable;
            set => BeastList.Interactable = value;
        }

        private void OnEnable()
        {
            BeastList.OnInspectingEvent += ShowBeastDetails;
            BeastList.OnBeastSelected += OnBeastSelected;
        }

        private void OnDisable()
        {
            BeastList.OnInspectingEvent -= ShowBeastDetails;
            BeastList.OnBeastSelected -= OnBeastSelected;
        }

        private void OnBeastSelected(IBeast beast)
        {
            BeastToUpgrade = beast;
        }

        private void ShowBeastDetails(UIBeastUpgradeListDetail ui)
        {
            UiBeastUpgradeDetail.SetupUI(ui.Beast);
        }

        public void InitBeast(List<Beast.Beast> beasts)
        {
            LeftPanel.SetActive(true);
            if (beasts.Count <= 0) return;

            BeastList.FillBeasts(beasts);
            Interactable = true;
            UiBeastUpgradeDetail.gameObject.SetActive(true);
        }
    }
}