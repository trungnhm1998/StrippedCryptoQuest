using System;
using System.Collections;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.System;
using IndiGames.Core.Common;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class EquipmentListPresenter : MonoBehaviour
    {
        public event Action<IUpgradeEquipment> OnSubmitItem;

        [SerializeField] private BlackSmithDialogsPresenter _dialogManager;
        [SerializeField] private UIEquipmentList _equipmentListUI;
        [SerializeField] private GameObject _equipmentUIObject;
        [SerializeField] private GameObject _equipmentDetailsObject;
        [SerializeField] private EquipmentDetailsPresenter _equipmentDetailsUI;

        [SerializeField] private LocalizedString _selectEquipmentMessage;

        private IUpgradeModel _upgradeModel;

        public UIEquipmentList EquipmentListUI => _equipmentListUI;
        public IUpgradeEquipment SelectedEquipment { get; set; }

        private void Awake()
        {
            _upgradeModel = GetComponent<IUpgradeModel>();
        }

        private void OnEnable()
        {
            _equipmentListUI.OnSelectedUpgradeItem += SelectedUpgradeItem;
            _equipmentListUI.OnSubmitUpgradeItem += OnItemSubmit;
        }

        private void OnDisable()
        {
            _equipmentListUI.OnSelectedUpgradeItem -= SelectedUpgradeItem;
            _equipmentListUI.OnSubmitUpgradeItem -= OnItemSubmit;
        }

        public void Show()
        {
            SetActiveUI(true);
            InitEquipmentsUI();
            ShowMessage();
        }

        public void Hide()
        {
            SetActiveUI(false);
        }

        public void SetInteractable(bool value)
        {
            _equipmentListUI.SetInteractable(value);
        }

        public void ShowMessage()
        {
            _dialogManager.Dialogue.SetMessage(_selectEquipmentMessage).Show();
        }

        private void InitEquipmentsUI()
        {
            StartCoroutine(CoInitUI());
        }

        private IEnumerator CoInitUI()
        {
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;
            yield return _upgradeModel.CoGetData(inventory);
            _equipmentListUI.InitUI(_upgradeModel);
        }

        private void SetActiveUI(bool value)
        {
            _equipmentUIObject.SetActive(value);
            _equipmentDetailsObject.SetActive(value);
        }

        private void SelectedUpgradeItem(UIUpgradeEquipment item)
        {
            SelectedEquipment = item.UpgradeEquipment;
            _equipmentDetailsUI.SetData(item.UpgradeEquipment.Equipment);
        }

        private void OnItemSubmit(UIUpgradeEquipment item)
        {
            OnSubmitItem?.Invoke(item.UpgradeEquipment);
        }
    }
}