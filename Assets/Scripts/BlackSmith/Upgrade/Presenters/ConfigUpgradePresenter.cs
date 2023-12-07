using System;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.BlackSmith.ScriptableObjects;
using CryptoQuest.BlackSmith.Upgrade.UI;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade.Presenters
{
    public class ConfigUpgradePresenter : MonoBehaviour
    {
        public event Action<IEquipment> ConfiguratedUpgrade;

        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _currencySO;
        [SerializeField] private EquipmentsPresenter _equipmentListPresenter;
        [SerializeField] private EquipmentDetailsPresenter _equipmentDetailsPresenter;
        [SerializeField] private UIUpgradeDetails _upgradeDetailsUI;

        [SerializeField] private UpgradeCostDatabase _costDatabase;

        private IEquipment _selectedEquipment;
        public IUpgradeEquipmentValidator _upgradeValidator = new UpgradeEquipmentValidator();

        public int LevelToUpgrade { get; private set; }
        public float GoldNeeded { get; private set; }

        private Dictionary<int, CostByRarity> _costDataDict = new();

        public bool IsUpgradeValid => _upgradeValidator.IsEnoughGoldToUpgrade(_wallet[_currencySO].Amount,
            GoldNeeded);


        private void Awake()
        {
            foreach (var data in _costDatabase.CostData)
            {
                _costDataDict.TryAdd(data.RarityID, data);
            }
        }

        private void OnEnable()
        {
            InitUI();
        }

        private void InitUI()
        {
            _selectedEquipment = _equipmentListPresenter.SelectedEquipment;
            LevelToUpgrade = _selectedEquipment.Level;

            SetLevelToUpgrade(1);
            UpdateUIs();
        }

        public void HandleNavigation(Vector2 direction)
        {
            var dir = (int)direction.y;
            if (_selectedEquipment == null || dir == 0) return;

            SetLevelToUpgrade(dir);
            UpdateUIs();
        }

        private void SetLevelToUpgrade(int modifyValue = 0)
        {
            if (!_upgradeValidator.CanUpgrade(_selectedEquipment, LevelToUpgrade + modifyValue))
                return;

            LevelToUpgrade += modifyValue;
            
            _costDataDict.TryGetValue(_selectedEquipment.Rarity.ID, out var data);
            GoldNeeded = data.GetCost(_selectedEquipment.Level, LevelToUpgrade);
        }

        private void UpdateUIs()
        {
            _upgradeDetailsUI.SetupUI(LevelToUpgrade, GoldNeeded, IsUpgradeValid);
            _equipmentDetailsPresenter.PreviewEquipmentAtLevel(LevelToUpgrade);
        }
    }
}