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
        public event Action<IUpgradeEquipment> ConfiguratedUpgrade;

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
        public IUpgradeEquipmentValidator _upgradeValidator = new UpgradeEquipmentValidator();

        public int LevelToUpgrade { get; private set; }
        public float GoldNeeded { get; private set; }

        private bool IsUpgradeValid => _upgradeValidator.IsEnoughGoldToUpgrade(_selectedEquipment,
            _wallet[_currencySO].Amount, LevelToUpgrade);

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
            LevelToUpgrade = _selectedEquipment.Level;

            SetActiveUI(true);
            RegistEvents();
            SetLevelToUpgrade(1);
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

        private void SetActiveUI(bool value)
        {
            _configUIObject.SetActive(value);
        }

        private void HandleNavigation(Vector2 direction)
        {
            var dir = (int)direction.y;
            if (_selectedEquipment == null || dir == 0) return;

            SetLevelToUpgrade(dir);
            UpdateUIs();
        }

        private void SetLevelToUpgrade(int modifyValue = 0)
        {
            if (!_upgradeValidator.CanUpgrade(_selectedEquipment.Equipment, LevelToUpgrade + modifyValue))
                return;

            LevelToUpgrade += modifyValue;
            GoldNeeded = _selectedEquipment.GetCost(_selectedEquipment.Level, LevelToUpgrade);
        }

        private void UpdateUIs()
        {
            _upgradeDetailsUI.SetupUI(LevelToUpgrade, GoldNeeded, IsUpgradeValid);
            _equipmentDetailsPresenter.PreviewEquipmentAtLevel(LevelToUpgrade);
        }

        private void OnConfiguratedUpgrade()
        {
            if (!IsUpgradeValid)
            {
                Debug.LogWarning($"Not enough gold!");
                return;
            }
            ConfiguratedUpgrade?.Invoke(_selectedEquipment);
        }
    }
}