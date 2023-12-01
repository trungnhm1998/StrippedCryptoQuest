using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Common;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory;
using IndiGames.Core.Common;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EvolveableEquipmentsPresenter : Presenter
    {
        [field: SerializeField, Header("Config")] public UIEvolveEquipmentList EvolveEquipmentListUI { get; private set; }
        [field: SerializeField] public BlackSmithDialogsPresenter DialogPresenter { get; private set; }

        [field: SerializeField, Header("Localization")] public LocalizedString SelectBaseMessage { get; private set; }
        [field: SerializeField] public LocalizedString SelectMaterialMessage { get; private set; }

        private IEvolvableModel _evolveModel;
        private Coroutine _coGetEquipment;

        private UIEquipmentItem _baseEquipment;
        public UIEquipmentItem BaseEquipment
        {
            get => _baseEquipment;
            set
            {
                _baseEquipment = value;
                OnSubmitBaseEquipment?.Invoke();
            }
        }

        private Stack<UIEquipmentItem> _materialEquipments = new();
        public Stack<UIEquipmentItem> MaterialEquipments => _materialEquipments;

        public int MaterialRequiredCount { get; private set; } = 1;

        public event Action OnSubmitBaseEquipment;
        public event Action OnSubmitMaterialEquipment;
        public event Action OnEquipmentRendered;
        public event Action<UIEquipmentItem> OnInspectingEquipmentItemChange;

        private void Awake() => _evolveModel = GetComponent<IEvolvableModel>();

        private void OnEnable()
        {
            _coGetEquipment = StartCoroutine(CoGetEquipment());
            EvolveEquipmentListUI.OnEquipmentRendered += HandleEquipmentRendered;
            EvolveEquipmentListUI.OnItemSelected += HandleInspectingEquipment;
            EvolveEquipmentListUI.OnItemSubmitted += HandleEquipmentSubmit;
        }

        private void OnDisable()
        {
            EvolveEquipmentListUI.OnEquipmentRendered -= HandleEquipmentRendered;
            EvolveEquipmentListUI.OnItemSelected -= HandleInspectingEquipment;
            EvolveEquipmentListUI.OnItemSubmitted -= HandleEquipmentSubmit;

            if (_coGetEquipment != null)
            {
                StopCoroutine(_coGetEquipment);
                _coGetEquipment = null;
            }
        }

        public void ReloadEquipments()
        {
            if (_coGetEquipment != null)
                StopCoroutine(_coGetEquipment);

            _coGetEquipment = StartCoroutine(CoGetEquipment());
        }

        private void HandleEquipmentRendered() => OnEquipmentRendered?.Invoke();

        private void HandleInspectingEquipment(UIEquipmentItem equipmentItem) => OnInspectingEquipmentItemChange.Invoke(equipmentItem);

        private void HandleEquipmentSubmit(UIEquipmentItem equipmentItem)
        {
            if (BaseEquipment == null)
            {
                DialogPresenter.Dialogue.SetMessage(SelectMaterialMessage);
                BaseEquipment = equipmentItem;
                BaseEquipment.SetAsBase();
                return;
            }

            if (BaseEquipment.EquipmentData.Equipment == equipmentItem.EquipmentData.Equipment)
                return;

            _materialEquipments.Push(equipmentItem);
            OnSubmitMaterialEquipment?.Invoke();
        }

        public IEnumerator CoGetEquipment()
        {
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;
            yield return _evolveModel.CoGetData(inventory);
            EvolveEquipmentListUI.RenderEquipments(_evolveModel.EvolvableEquipments);
        }

        public void ClearMaterialEquipmentsIfExist()
        {
            while (_materialEquipments.Count > 0)
            {
                UIEquipmentItem popItem = _materialEquipments.Pop();
                popItem.ResetItemStates();
            }
        }

        public void RemoveBaseEquipmentIfExist()
        {
            if (BaseEquipment != null)
            {
                BaseEquipment.ResetItemStates();
                BaseEquipment = null;
            }
        }
    }
}
