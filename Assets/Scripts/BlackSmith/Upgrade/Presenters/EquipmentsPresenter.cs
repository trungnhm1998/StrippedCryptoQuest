using System;
using System.Collections;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.BlackSmith.Upgrade.UI;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Common;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade.Presenters
{
    public class EquipmentsPresenter : MonoBehaviour
    {
        public event Action<IEquipment> OnSubmitItem;

        [SerializeField] private UIEquipmentList _equipmentListUI;
        [SerializeField] private GameObject _equipmentDetailsObject;
        [SerializeField] private EquipmentDetailsPresenter _equipmentDetailsUI;

        [SerializeField] private LocalizedString _selectEquipmentMessage;

        private IUpgradeModel _upgradeModel;

        public IEquipment SelectedEquipment { get; set; }

        private void Awake()
        {
            _upgradeModel = GetComponent<IUpgradeModel>();
        }

        private void OnEnable()
        {
            _equipmentListUI.OnSelectedUpgradeItem += SelectedUpgradeItem;
            _equipmentListUI.OnSubmitUpgradeItem += OnItemSubmit;

            _equipmentListUI.ResetSelected();
            SetActiveDetailUI(true);
            InitEquipmentsUI();
        }

        private void OnDisable()
        {
            _equipmentListUI.OnSelectedUpgradeItem -= SelectedUpgradeItem;
            _equipmentListUI.OnSubmitUpgradeItem -= OnItemSubmit;
            SetActiveDetailUI(false);
        }

        public void SetInteractable(bool value)
        {
            _equipmentListUI.SetInteractable(value);
        }

        private void InitEquipmentsUI()
        {
            StartCoroutine(CoInitUI());
        }

        private const float ERROR_PRONE_DELAY = 0.05f;
        private IEnumerator CoInitUI()
        {
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;
            yield return _upgradeModel.CoGetData(inventory);
            _equipmentListUI.InitUI(_upgradeModel);

            yield return new WaitForSeconds(ERROR_PRONE_DELAY);
            SetInteractable(true);
        }

        private void SetActiveDetailUI(bool value)
        {
            _equipmentDetailsObject.SetActive(value);
        }

        private void SelectedUpgradeItem(UIUpgradeEquipment item)
        {
            SelectedEquipment = item.UpgradeEquipment;
            _equipmentDetailsUI.SetData(item.UpgradeEquipment);
        }

        private void OnItemSubmit(UIUpgradeEquipment item)
        {
            OnSubmitItem?.Invoke(item.UpgradeEquipment);
        }
    }
}