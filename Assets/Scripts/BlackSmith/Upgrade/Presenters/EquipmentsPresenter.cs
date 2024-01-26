using System;
using System.Collections;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.BlackSmith.Upgrade.UI;
using CryptoQuest.Item.Equipment;
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
            
            SetActiveDetailUI(true);
            InitEquipmentsUI();
        }

        public void CancelUI()
        {
            SetInteractable(true);
            _equipmentListUI.ResetSelected();
            _equipmentDetailsUI.ResetPreviews();
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
            yield return _upgradeModel.CoGetData();

            var hasEquipments = _upgradeModel.Equipments.Count > 0;
            _equipmentDetailsUI.gameObject.SetActive(hasEquipments);

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