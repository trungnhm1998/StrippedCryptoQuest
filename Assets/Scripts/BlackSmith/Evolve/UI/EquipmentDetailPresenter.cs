using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Tooltips;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EquipmentDetailPresenter : MonoBehaviour
    {
        [field: SerializeField] public UIEvolvableEquipmentList EquipmentsPresenterUI { get; private set; }
        [field: SerializeField] public UIEvolveEquipmentTooltip EquipmentDetailUI { get; private set; }
        [field: SerializeField] public UIStarsWithPreview StarsUI { get; private set; }
        [SerializeField] private AttributeConfigMapping _attributeConfigMapping;

        private List<UIAttributePreviewableUnknown> _attributePreviewUIs = new();

        public IEquipment Equipment { get; private set; }

        private void OnDisable()
        {
            Equipment = null;
        }

        public void ShowEquipment(IEquipment equipment)
        {
            Equipment = equipment;
            SetActiveTooltip(false);
            EquipmentDetailUI.SetEquipment(Equipment);
            SetActiveTooltip(true);
        }

        private void SetActiveTooltip(bool value)
        {
            EquipmentDetailUI.gameObject.SetActive(value);
        }

        public void ShowPreview(IEvolvableEquipment equipment)
        {
            StarsUI.SetPreviewStars(equipment.Stars, equipment.AfterStars);
            EquipmentDetailUI.SetPreviewLevel(equipment.MinLevel, equipment.MaxLevel);
            SetupPreviewerUIs();
        }

        public void HidePreview() => StarsUI.HidePreview();

        private void SetupPreviewerUIs()
        {
            _attributePreviewUIs.Clear();
            _attributePreviewUIs = GetComponentsInChildren<UIAttributePreviewableUnknown>().ToList();

            foreach (var attribute in Equipment.Data.Stats)
            {
                if (!_attributeConfigMapping.TryGetMap(attribute.Attribute, out var config))
                    continue;
                var previewUI = _attributePreviewUIs.Where(u => u.LocalizeAttributeName == config.Name).First();
                previewUI.SetPreviewUnknownValue();
            }
        }
    }
}
