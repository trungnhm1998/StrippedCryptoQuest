using System;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.BlackSmith.Upgrade.StateMachine;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UpgradePresenter : MonoBehaviour
    {
        [SerializeField] private UIUpgradeEquipment _upgradeEquipment;
        [SerializeField] private UIEquipmentDetails _equipmentDetail;
        [SerializeField] private BlackSmithInputManager _input;
        [SerializeField] private CurrencyPresenter _currencyController;
        [SerializeField] private UpgradeStateController _upgradeController;
        private IUpgradeEquipment _equipmentData;
        private IUpgradeModel _upgradeModel;
        private UIUpgradeItem _item;
        private bool _isSelectedEquipment;
        private int _level;

        private void OnEnable()
        {
            Init();
            _upgradeEquipment.OnSelected += OnItemSelected;
            _upgradeEquipment.OnSubmit += OnItemSubmit;
            _upgradeEquipment.GotLevelEvent += GetLevelToUpgrade;
            _input.NavigateEvent += HandleNavigation;
            _input.SubmitEvent += UpgradeEquipment;
            _currencyController.OnSendSuccess += NotifySuccess;
            _currencyController.OnSendFailed += NotifyFailed;
        }

        private void OnDisable()
        {
            _upgradeEquipment.OnSelected -= OnItemSelected;
            _upgradeEquipment.OnSubmit -= OnItemSubmit;
            _upgradeEquipment.GotLevelEvent -= GetLevelToUpgrade;
            _input.SubmitEvent -= UpgradeEquipment;
            _input.NavigateEvent -= HandleNavigation;
            _currencyController.OnSendSuccess -= NotifySuccess;
            _currencyController.OnSendFailed -= NotifyFailed;
        }

        private void GetLevelToUpgrade(int level) => _level = level;

        private void Awake()
        {
            _upgradeModel = GetComponent<IUpgradeModel>();
        }

        private void OnItemSelected(UIUpgradeItem item)
        {
            _item = item;
            LoadEquipmentDetail();
        }

        public void Init()
        {
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;
            _upgradeModel.CoGetData(inventory);
            _upgradeEquipment.InstantiateData(_upgradeModel);
            _isSelectedEquipment = false;
        }

        private void OnItemSubmit(UIUpgradeItem item)
        {
            _upgradeEquipment.SelectedEquipment(item);
            _equipmentData = item.UpgradeEquipment;
            _isSelectedEquipment = true;
        }

        private void UpgradeEquipment()
        {
            if (!_isSelectedEquipment) return;
            //TODO: Import Master data to calculate cost to upgrade multi level
            _currencyController.CurrencyNeeded(_equipmentData.Cost * _level, 0); // (0 is metad to upgrade but this function don't use metad)
        }

        private void HandleNavigation(Vector2 direction)
        {
            float currentGold = _currencyController.Gold;
            if (_equipmentData == null || (int)direction.y == 0) return;
            _upgradeEquipment.SetValue((int)direction.y, _equipmentData, currentGold);
        }

        private void LoadEquipmentDetail()
        {
            if (_item == null) return;
            _equipmentDetail.RenderData(_item.UpgradeEquipment.Equipment);
        }

        private void NotifySuccess()
        {
            _upgradeController.OnUpgradeSuccess?.Invoke();
            _upgradeEquipment.SetLevel(_equipmentData, _equipmentDetail);
            Debug.Log("Upgrade Success!!!"); //TODO: Add confirm UI & Dialog 
        }

        private void NotifyFailed()
        {
            Debug.Log("Upgrade Fail!!!"); //TODO: Add confirm UI & Dialog 
        }
    }
}