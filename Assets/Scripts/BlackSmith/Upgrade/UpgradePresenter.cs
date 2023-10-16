using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UpgradePresenter : MonoBehaviour
    {
        [SerializeField] private UIUpgradeEquipment _upgradeEquipment;
        [SerializeField] private UIEquipmentDetails _equipmentDetail;
        [SerializeField] private BlackSmithInputManager _input;
        [SerializeField] private List<UIUpgradeCharacter> _listCharacter;
        private IPartyController _partyController;
        private IUpgradeEquipment _equipmentData;
        private IUpgradeModel _upgradeModel;

        private void OnEnable()
        {
            Init();
            UIUpgradeItem.SelectedItemEvent += OnSelecetedEquipment;
            _input.NavigateEvent += SelectLevelToUpgrade;
        }

        private void OnDisable()
        {
            UIUpgradeItem.SelectedItemEvent -= OnSelecetedEquipment;
            _input.NavigateEvent -= SelectLevelToUpgrade;
        }

        private void Awake()
        {
            _upgradeModel = GetComponent<IUpgradeModel>();
        }

        public void Init()
        {
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;
            _partyController = ServiceProvider.GetService<IPartyController>();
            _upgradeModel.CoGetData(inventory);
            _upgradeEquipment.InstantiateData(_upgradeModel);
            LoadCharacterDetail();
        }

        private void OnSelecetedEquipment(UIUpgradeItem item)
        {
            _upgradeEquipment.SelectedEquipment(item);
            _equipmentData = item.UpgradeEquipment;
            LoadEquipmentDetail();
        }

        public void UpgradeEquipment()
        {
            _upgradeEquipment.SetLevel(_equipmentData);
        }

        private void SelectLevelToUpgrade(Vector2 direction)
        {
            LoadEquipmentDetail();
            if (_equipmentData == null || (int)direction.y == 0) return;
            _upgradeEquipment.SetValue((int)direction.y, _equipmentData);
        }

        private void LoadEquipmentDetail()
        {
            var currentEquipment = EventSystem.current.currentSelectedGameObject?.GetComponent<UIUpgradeItem>();
            if (currentEquipment == null) return;
            _equipmentDetail.RenderData(currentEquipment.UpgradeEquipment.Equipment);
        }

        private void LoadCharacterDetail()
        {
            for (int i = 0; i < _partyController.Slots.Length; i++)
            {
                _listCharacter[i].LoadCharacterDetail(_partyController.Slots[i].HeroBehaviour);
            }
        }
    }
}