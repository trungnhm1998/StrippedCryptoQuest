using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.BlackSmith.Commons.UI;
using UnityEngine;
using UnityEngine.Localization;
using System;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EquipmentsPresenter : MonoBehaviour
    {
        [SerializeField, Header("Configs")]
        private UIEvolvableEquipmentList _evolvableEquipmentsUI;

        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _goldCurrencySO;
        [SerializeField] private CurrencySO _diamondCurrencySO;

        [field: SerializeField, Header("Localization")]
        public LocalizedString SelectBaseMessage { get; private set; }

        [field: SerializeField] public LocalizedString SelectMaterialMessage { get; private set; }

        private IEvolvableModel _model;

        public IEvolvableModel EvolvableModel => _model;

        private List<IEvolvableEquipmentItem> _evolvableEquipmentItems = new();

        private void Awake() => _model = GetComponent<IEvolvableModel>();

        public void InitEquipments(IEvolvableInfo[] infos)
        {
            _model.Init();
            var equipments = _model.GetEvolableEquipments();

            _evolvableEquipmentItems.Clear();

            foreach (var equipment in equipments)
            {
                var evolveInfo = infos.FirstOrDefault(f => f.BeforeStars == equipment.Data.Stars && f.Rarity == equipment.Data.Rarity.ID);
                if (evolveInfo == null) continue;
                var evolvableEquipmentItem = new EvolvableEquipmentItem
                {
                    Equipment = equipment,
                    GoldCheck = new CurrencyValueEnough()
                    {
                        Value = evolveInfo.Gold,
                        IsEnough = _wallet[_goldCurrencySO].Amount >= evolveInfo.Gold
                    },
                    DiamondCheck = new CurrencyValueEnough()
                    {
                        Value = evolveInfo.Metad,
                        IsEnough = _wallet[_diamondCurrencySO].Amount >= evolveInfo.Metad
                    }
                };
                _evolvableEquipmentItems.Add(evolvableEquipmentItem);
            }
        }

        public void RenderEquipmentsForBaseItemSelection()
        {
            _evolvableEquipmentsUI.ClearEquipmentsWithException();
            _evolvableEquipmentsUI.RenderEquipmentsWithException(_evolvableEquipmentItems);
        }

        public void RenderEquipmentsForMaterialItemSelection(UIEquipmentItem baseEquipmentItem)
        {
            AnchorBaseEquipment(baseEquipmentItem);

            _evolvableEquipmentsUI.ClearEquipmentsWithException(baseEquipmentItem);
            var evolvableEquipmentItemsExceptBase = _evolvableEquipmentItems.Where(e => e.Equipment.Data.ID == baseEquipmentItem.Equipment.Data.ID && e.Equipment.Id != baseEquipmentItem.Equipment.Id).ToList();
            _evolvableEquipmentsUI.RenderEquipmentsWithException(evolvableEquipmentItemsExceptBase);
        }

        public void ResetAnchorIfExist(UIEquipmentItem baseEquipmentItem)
        {
            if (baseEquipmentItem == null) return;
            baseEquipmentItem.transform.SetParent(_evolvableEquipmentsUI.Content);
        }

        private void AnchorBaseEquipment(UIEquipmentItem baseEquipmentItem)
        {
            baseEquipmentItem.transform.SetParent(_evolvableEquipmentsUI.Viewport);
            baseEquipmentItem.transform.SetAsFirstSibling();
            baseEquipmentItem.ButtonUI.interactable = false;
            baseEquipmentItem.BaseTag.SetActive(true);
            baseEquipmentItem.SetSelected(true);
        }
    }
}