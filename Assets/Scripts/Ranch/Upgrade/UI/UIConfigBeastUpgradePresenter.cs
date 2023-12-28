using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Inventory.Currency;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Ranch.ScriptableObject;
using CryptoQuest.Ranch.Upgrade.Presenters;
using UnityEngine;
using BeastData = CryptoQuest.Beast.Beast;

namespace CryptoQuest.Ranch.Upgrade.UI
{
    public class UIConfigBeastUpgradePresenter : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private WalletSO _wallet;

        [SerializeField] private CurrencySO _currencySO;

        [Header("Event")]
        [SerializeField] private BeastUpgradeCostDatabase _costDatabase;

        [SerializeField] private CalculatorBeastStatsSO _calculatorBeastStatsSo;

        [Header("Components")]
        [SerializeField] private UIConfigBeastUpgradeDetail _beastUpgradeDetail;

        [SerializeField] private UpgradePresenter _upgradePresenter;
        [SerializeField] private UIBeastUpgradeDetail _uiBeastUpgradeDetail;
        public int LevelToUpgrade { get; private set; }
        public float GoldNeeded { get; private set; }

        public bool IsUpgradeValid =>
            _validator.IsEnoughGoldToUpgrade(_wallet[_currencySO].Amount, GoldNeeded) &&
            _validator.CanUpgrade(_selectedBeast, LevelToUpgrade);

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
            _uiBeastUpgradeDetail.ResetAttributesUI();
            _calculatorBeastStatsSo.RaiseEvent(_selectedBeast);
            _beastUpgradeDetail.gameObject.SetActive(false);
        }

        public void HandleNavigation(Vector2 direction)
        {
            var dir = (int)direction.y;
            if (dir == 0) return;

            SetLevelToUpgrade(dir);
        }

        private void SetLevelToUpgrade(int value = 0)
        {
            if (!_validator.CanUpgrade(_selectedBeast, LevelToUpgrade + value)) return;

            LevelToUpgrade += value;

            _costDataDict.TryGetValue(_selectedBeast.Level, out var data);
            GoldNeeded = data.GetCost(_selectedBeast.Level, LevelToUpgrade);
            UpdateUIs();
        }

        private void UpdateUIs()
        {
            _beastUpgradeDetail.SetupUI(LevelToUpgrade, GoldNeeded, IsUpgradeValid);


            _uiBeastUpgradeDetail.ResetAttributesUI();
            _calculatorBeastStatsSo.RaiseEvent(_selectedBeast);

            BeastData beast = CreateBeastFromSelectedBeast();
            _uiBeastUpgradeDetail.PreviewBeastStats(true);
            _calculatorBeastStatsSo.RaiseEvent(beast);
        }

        private BeastData CreateBeastFromSelectedBeast()
        {
            return new BeastData
            {
                Level = LevelToUpgrade,
                Stats = _selectedBeast.Stats,
                Elemental = _selectedBeast.Elemental,
                Class = _selectedBeast.Class,
                Stars = _selectedBeast.Stars,
                MaxLevel = _selectedBeast.MaxLevel,
                Passive = _selectedBeast.Passive,
                Id = _selectedBeast.Id,
                BeastId = _selectedBeast.BeastId,
                Type = _selectedBeast.Type
            };
        }
    }
}