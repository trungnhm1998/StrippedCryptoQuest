using System;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class ConfigUpgradePresenter : MonoBehaviour
    {
        public event Action<IUpgradeEquipment, int> ConfiguratedUpgrade;

        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _currencySO;
        [SerializeField] private EquipmentListPresenter _equipmentListPresenter;
        [SerializeField] private EquipmentDetailsPresenter _equipmentDetailsPresenter;
        [SerializeField] private MerchantsInputManager _input;
        [SerializeField] private BlackSmithDialogsPresenter _dialogManager;
        [SerializeField] private GameObject _configUIObject;
        [SerializeField] private UIUpgradeDetails _upgradeDetailsUI;

        [SerializeField] private LocalizedString _upgradeMessage;

        private IUpgradeEquipment _selectedEquipment;

        private void OnDisable()
        {
            UnRegistEvent();
        }

        private void RegistEvents()
        {
            _input.NavigateEvent += HandleNavigation;
            _input.SubmitEvent += OnConfiguratedUpgrade;
        }

        private void UnRegistEvent()
        {
            _input.NavigateEvent -= HandleNavigation;
            _input.SubmitEvent -= OnConfiguratedUpgrade;
        }

        public void Show()
        {
            _selectedEquipment = _equipmentListPresenter.SelectedEquipment;

            SetActiveUI(true);
            InitUpgradeDetailUI();
            RegistEvents();
            UpdateUIs();

            _dialogManager.Dialogue.SetMessage(_upgradeMessage).Show();
        }

        public void Hide()
        {
            SetActiveUI(false);
            UnRegistEvent();
        }
        
        public void CancelUI()
        {
            _equipmentDetailsPresenter.ResetPreviews();
        }

        private void InitUpgradeDetailUI()
        {
            _upgradeDetailsUI.SetupUI(_selectedEquipment, _wallet[_currencySO].Amount);
        }

        private void SetActiveUI(bool value)
        {
            _configUIObject.SetActive(value);
        }

        private void HandleNavigation(Vector2 direction)
        {
            var dir = (int)direction.y;
            if (_selectedEquipment == null || dir == 0) return;

            UpdateUIs(dir);
        }

        private void UpdateUIs(int modifyValue = 0)
        {
            _upgradeDetailsUI.UpdateValue(modifyValue, _selectedEquipment);
            _equipmentDetailsPresenter.PreviewEquipmentAtLevel(_upgradeDetailsUI.LevelToUpgrade);
        }

        private void OnConfiguratedUpgrade()
        {
            ConfiguratedUpgrade?.Invoke(_selectedEquipment, _upgradeDetailsUI.LevelToUpgrade);
        }
    }
}