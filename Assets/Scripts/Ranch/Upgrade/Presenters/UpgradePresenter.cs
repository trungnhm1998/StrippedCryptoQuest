using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Ranch.Upgrade.UI;
using UnityEngine;

namespace CryptoQuest.Ranch.Upgrade.Presenters
{
    public class UpgradePresenter : MonoBehaviour
    {
        [field: SerializeField] public UIConfigBeastUpgradePresenter UIConfigBeastUpgradePresenter { get; private set; }

        [SerializeField] private UIBeastUpgradeList _beastList;
        [SerializeField] private UIBeastUpgradeDetail _uiBeastEvolveDetail;

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
            _beastList.FillBeasts(beasts);
            Interactable = true;
        }
    }
}