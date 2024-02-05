using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Ranch.Upgrade.UI;
using UnityEngine;

namespace CryptoQuest.Ranch.Upgrade.Presenters
{
    public class UpgradePresenter : MonoBehaviour
    {
        public IBeast BeastToUpgrade { get; private set; } = NullBeast.Instance;

        public bool Interactable
        {
            get => _beastList.Interactable;
            set => _beastList.Interactable = value;
        }

        [SerializeField] private UIResultUpgradeBeastList _resultBeast;
        [SerializeField] private UIBeastUpgradeList _beastList;
        [SerializeField] private UIBeastUpgradeDetail _uiBeastDetail;
        [SerializeField] private UIBeastUpgradeDetail _uiBeastResultDetail;
        [SerializeField] private CalculatorBeastStatsSO _calculatorBeastStatsSo;
        [SerializeField] private BeastInventorySO _beastInventory;
        [SerializeField] private GameObject _leftPanel;

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
            _uiBeastDetail.SetupUI(ui.Beast);
        }

        public void InitBeast(List<IBeast> beasts)
        {
            _leftPanel.SetActive(true);
            _beastList.Clear();
            if (beasts.Count <= 0) return;

            Interactable = true;
            _beastList.FillBeasts(beasts);
        }

        public void ActiveBeastDetail(bool value)
        {
            _uiBeastDetail.gameObject.SetActive(value);
        }

        public void ShowResult()
        {
            _resultBeast.Show();
            _leftPanel.SetActive(false);
            ActiveBeastDetail(false);

            IBeast beast = _beastInventory.GetBeast(BeastToUpgrade.Id);

            _uiBeastResultDetail.SetupUI(beast);
            _calculatorBeastStatsSo.RaiseEvent(beast);
        }

        public void HideResult()
        {
            _resultBeast.Hide();
        }
    }
}