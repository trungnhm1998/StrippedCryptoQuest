using System.Collections.Generic;
using AssetReferenceSprite;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Character;
using CryptoQuest.UI.Tooltips.Equipment;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using System.Linq;
using CryptoQuest.AbilitySystem.Attributes;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class EquipmentDetailsPresenter : MonoBehaviour, ITooltipEquipmentProvider
    {
        [SerializeField] private UIUpgradeEquipmentTooltip _equipmentTooltip;
        [SerializeField] private AttributeConfigMapping _attributeConfigMapping;

        private Dictionary<AttributeWithValue, UIPreviewableAttribute> _attributePreviewUIs = new();

        public IEquipment Equipment { get; private set; }

        public void SetData(IEquipment equipment)
        {
            if (Equipment != null && equipment == Equipment) return;
            Equipment = equipment;
            SetActiveTooltip(false);
            SetActiveTooltip(true);
            SetupPreviewerUIs();
        }

        private void SetActiveTooltip(bool value)
        {
            _equipmentTooltip.gameObject.SetActive(value);
        }

        public void PreviewEquipmentAtLevel(int level)
        {
            foreach (var attribute in Equipment.Data.Stats)
            {
                var newValue = attribute.Value;
                newValue += (level - 1) * Equipment.Data.ValuePerLvl;
                _attributePreviewUIs.TryGetValue(attribute, out var previewUI);
                previewUI.SetPreviewValue(newValue);
            }
        }

        public void ResetPreviews()
        {
            foreach (var ui in _attributePreviewUIs.Values)
            {
                ui.ResetPreviewUI();
            }
        }

        private void SetupPreviewerUIs()
        {
            _attributePreviewUIs.Clear();
            var previewUIs = GetComponentsInChildren<UIPreviewableAttribute>();

            foreach (var attribute in Equipment.Data.Stats)
            {
                if (!_attributeConfigMapping.TryGetMap(attribute.Attribute, out var config))
                    continue;
                var attributeName = config.Name.GetLocalizedString();
                var previewUI = previewUIs.Where(u => u.AttributeName == attributeName).First();
                _attributePreviewUIs.Add(attribute, previewUI);
            }
        }
    }
}
