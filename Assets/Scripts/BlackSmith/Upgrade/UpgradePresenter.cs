using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.BlackSmith.Upgrade.StateMachine;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UpgradePresenter : MonoBehaviour
    {
        [SerializeField] private BlackSmithDialogsPresenter _dialogManager;
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
        
        [Header("Localization")]
        [SerializeField] private LocalizedString _selectEquipmentMessage;
        [SerializeField] private LocalizedString _upgradeMessage;
        [SerializeField] private LocalizedString _confirmUpgradeMessage;
        [SerializeField] private LocalizedString _resultMessage;

        private void OnEnable()
        {
            Init();
            _upgradeEquipment.OnSelected += OnItemSelected;
            _upgradeEquipment.OnSubmit += OnItemSubmit;
            _upgradeEquipment.GotLevelEvent += GetLevelToUpgrade;
            _input.NavigateEvent += HandleNavigation;
            _input.SubmitEvent += UpgradeEquipment;
            _input.CancelEvent += ExitUpgrade;
            _currencyController.OnSendSuccess += NotifySuccess;
            _dialogManager.ConfirmYesEvent += ProceedUpgrade;
            _dialogManager.ConfirmNoEvent += CancelUpgrade;
        }

        private void ExitUpgrade()
        {
            if (!_isSelectedEquipment) return;
            _upgradeController.ExitUpgradeEvent?.Invoke();
        }

        private void OnDisable()
        {
            _upgradeEquipment.OnSelected -= OnItemSelected;
            _upgradeEquipment.OnSubmit -= OnItemSubmit;
            _upgradeEquipment.GotLevelEvent -= GetLevelToUpgrade;
            _input.SubmitEvent -= UpgradeEquipment;
            _input.NavigateEvent -= HandleNavigation;
            _input.CancelEvent -= ExitUpgrade;
            _currencyController.OnSendSuccess -= NotifySuccess;
            _dialogManager.ConfirmYesEvent -= ProceedUpgrade;
            _dialogManager.ConfirmNoEvent -= CancelUpgrade;
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
            _dialogManager.Dialogue.SetMessage(_selectEquipmentMessage).Show();
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;
            _upgradeModel.CoGetData(inventory);
            _upgradeEquipment.InstantiateData(_upgradeModel);
            _isSelectedEquipment = false;
        }

        private void OnItemSubmit(UIUpgradeItem item)
        {
            float currentGold = _currencyController.Gold;
            _dialogManager.Dialogue.SetMessage(_upgradeMessage).Show();
            _upgradeEquipment.SelectedEquipment(item, currentGold);
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
            if(!_isSelectedEquipment) return;
            if (_equipmentData == null || (int)direction.y == 0) return;
            _upgradeEquipment.SetValue((int)direction.y, _equipmentData);
        }

        private void LoadEquipmentDetail()
        {
            if (_item == null) return;
            _equipmentDetail.RenderData(_item.UpgradeEquipment.Equipment);
        }

        private void NotifySuccess()
        {
            _isSelectedEquipment = false;
            _dialogManager.Dialogue.Hide();
            _dialogManager.ShowConfirmDialog(_confirmUpgradeMessage);
            _upgradeEquipment.ShowConfirmPanel(true);
        }

        public void ProceedUpgrade()
        {
            _upgradeEquipment.ShowConfirmPanel(false);
            _upgradeController.UpgradeEvent?.Invoke();
            _upgradeEquipment.SetLevel(_equipmentData, _equipmentDetail);
            _dialogManager.Dialogue.SetMessage(_resultMessage).Show();
            _currencyController.UpdateCurrencyAmout();
        }

        public void CancelUpgrade()
        {
            _upgradeEquipment.ShowConfirmPanel(false);
            _isSelectedEquipment = true;
        }
    }
}