using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Ranch.Upgrade.UI;
using UnityEngine;

namespace CryptoQuest.Ranch.Upgrade.Presenters
{
    public class UpgradePresenter : MonoBehaviour
    {
        [field: SerializeField] public UIConfigBeastUpgradePresenter ConfigBeast { get; private set; }
        [field: SerializeField] public UIResultUpgradeBeastList ResultBeast { get; private set; }

        [SerializeField] private UIBeastUpgradeList _beastList;
        [SerializeField] private UIBeastUpgradeDetail _uiBeastEvolveDetail;
        [SerializeField] private GameObject _rightPanel;

        public IBeast BeastToUpgrade { get; set; }

        public bool Interactable
        {
            get => _beastList.Interactable;
            set => _beastList.Interactable = value;
        }

        private void OnEnable()
        {
            _beastList.OnInspectingEvent += ShowBeastDetails;
            _beastList.OnBeastSelected += OnBeastSelected;
        }

        private void OnDisable()
        {
            _beastList.OnInspectingEvent -= ShowBeastDetails;
            _beastList.OnBeastSelected -= OnBeastSelected;
        }

        private void OnBeastSelected(IBeast beast)
        {
            BeastToUpgrade = beast;
        }

        private void ShowBeastDetails(UIBeastUpgradeListDetail ui)
        {
            _uiBeastEvolveDetail.SetupUI(ui.Beast);
        }

        public void InitBeast(List<Beast.Beast> beasts)
        {
            if (beasts.Count == 0) return;

            _beastList.FillBeasts(beasts);
            _rightPanel.SetActive(true);
            Interactable = true;
        }
    }
}