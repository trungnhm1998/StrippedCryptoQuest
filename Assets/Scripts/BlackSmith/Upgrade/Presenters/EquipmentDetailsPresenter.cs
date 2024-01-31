using System.Collections.Generic;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Tooltips.Equipment;
using UnityEngine;
using System.Linq;
using CryptoQuest.AbilitySystem.Attributes;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using CryptoQuest.BlackSmith.Upgrade.UI;

namespace CryptoQuest.BlackSmith.Upgrade.Presenters
{
    public class EquipmentDetailsPresenter : MonoBehaviour, ITooltipEquipmentProvider
    {
        [SerializeField] private UIUpgradeEquipmentTooltip _equipmentTooltip;
        [SerializeField] private UIPreviewUpgradeLevel _previewLevelUI;
        [SerializeField] private AttributeConfigMapping _attributeConfigMapping;

        private Dictionary<AttributeWithValue, UIPreviewableAttribute> _attributePreviewUIs = new();

        public IEquipment Equipment { get; private set; }

        private void OnDisable()
        {
            Equipment = null;
        }

        public void SetData(IEquipment equipment)
        {
            if (Equipment != null && equipment == Equipment) return;
            Equipment = equipment;
            SetActiveTooltip(false);
            SetActiveTooltip(true);
            SetupPreviewerUIs();
            _previewLevelUI.SetCurrentLevel(Equipment.Level, Equipment.Data.MaxLevel);
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
                newValue += (level - Equipment.Level) * Equipment.Data.ValuePerLvl;
                _attributePreviewUIs.TryGetValue(attribute, out var previewUI);
                previewUI.SetPreviewValue(Mathf.FloorToInt(newValue));
            }

            _previewLevelUI.SetPreviewLevel(level, Equipment.Data.MaxLevel);
        }

        public void ResetPreviews()
        {
            foreach (var ui in _attributePreviewUIs.Values)
            {
                ui.ResetPreviewUI();
            }

            _previewLevelUI.ResetPreviewUI();
        }

        private void SetupPreviewerUIs()
        {
            _attributePreviewUIs.Clear();
            var previewUIs = GetComponentsInChildren<UIPreviewableAttribute>();

            foreach (var attribute in Equipment.Data.Stats)
            {
                if (!_attributeConfigMapping.TryGetMap(attribute.Attribute, out var config))
                    continue;
                var previewUI = previewUIs.Where(u => u.LocalizeAttributeName == config.Name).First();
                _attributePreviewUIs.Add(attribute, previewUI);
            }
        }
    }
}
