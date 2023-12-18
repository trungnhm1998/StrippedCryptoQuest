using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Ranch.ScriptableObject;
using CryptoQuest.Ranch.Upgrade.Presenters;
using UnityEngine;

namespace CryptoQuest.Ranch.Upgrade.UI
{
    public class UIConfigBeastUpgradePresenter : MonoBehaviour
    {
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _currencySO;
        [SerializeField] private UIConfigBeastUpgradeDetail _beastUpgradeDetail;
        [SerializeField] private BeastUpgradeCostDatabase _costDatabase;
        [SerializeField] private UpgradePresenter _upgradePresenter;
        public int LevelToUpgrade { get; private set; }
        public float GoldNeeded { get; private set; }
        public bool IsUpgradeValid => _validator.IsEnoughGoldToUpgrade(_wallet[_currencySO].Amount, GoldNeeded);

        private Dictionary<int, BeastCost> _costDataDict = new();
        private IBeast _selectedBeast = NullBeast.Instance;
        private readonly IBeastUpgradeValidator _validator = new BeastUpgradeValidator();

        private void Awake()
        {
            for (int i = 0; i < _costDatabase.CostData.Costs.Length; i++)
            {
                _costDataDict.TryAdd(i, _costDatabase.CostData);
            }
        }

        public void InitUI()
        {
            _selectedBeast = _upgradePresenter.BeastToUpgrade;
            LevelToUpgrade = _selectedBeast.Level;

            _beastUpgradeDetail.gameObject.SetActive(true);
            SetLevelToUpgrade(1);
            UpdateUIs();
        }

        public void DeInitUI()
        {
            _beastUpgradeDetail.gameObject.SetActive(false);
        }

        public void HandleNavigation(Vector2 direction)
        {
            var dir = (int)direction.y;
            if (dir == 0) return;

            SetLevelToUpgrade(dir);
            UpdateUIs();
        }

        private void SetLevelToUpgrade(int value = 0)
        {
            if (!_validator.CanUpgrade(_selectedBeast, LevelToUpgrade + value)) return;

            LevelToUpgrade += value;

            _costDataDict.TryGetValue(_selectedBeast.Level, out var data);
            GoldNeeded = data.GetCost(_selectedBeast.Level, LevelToUpgrade);
        }

        private void UpdateUIs()
        {
            _beastUpgradeDetail.SetupUI(LevelToUpgrade, GoldNeeded, IsUpgradeValid);
        }
    }
}