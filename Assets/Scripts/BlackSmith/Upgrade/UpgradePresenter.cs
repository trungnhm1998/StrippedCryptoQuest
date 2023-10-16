using CryptoQuest.BlackSmith.Interface;
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
        private IUpgradeEquipment _equipmentData;
        private IUpgradeModel _upgradeModel;
        private UIUpgradeItem _item;

        private void OnEnable()
        {
            Init();
            _upgradeEquipment.OnSelected += OnItemSelected;
            _upgradeEquipment.OnSubmit += OnItemSubmit;
            _input.NavigateEvent += HandleNavigation;
        }

        private void OnDisable()
        {
            _upgradeEquipment.OnSelected -= OnItemSelected;
            _upgradeEquipment.OnSubmit -= OnItemSubmit;
            _input.NavigateEvent -= HandleNavigation;
        }

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
        }

        private void OnItemSubmit(UIUpgradeItem item)
        {
            _upgradeEquipment.SelectedEquipment(item);
            _equipmentData = item.UpgradeEquipment;
        }

        public void UpgradeEquipment()
        {
            _upgradeEquipment.SetLevel(_equipmentData);
            LoadEquipmentDetail();
        }

        private void HandleNavigation(Vector2 direction)
        {
            if (_equipmentData == null || (int)direction.y == 0) return;
            _upgradeEquipment.SetValue((int)direction.y, _equipmentData);
        }

        private void LoadEquipmentDetail()
        {
            if (_item == null) return;
            _equipmentDetail.RenderData(_item.UpgradeEquipment.Equipment);
        }
    }
}