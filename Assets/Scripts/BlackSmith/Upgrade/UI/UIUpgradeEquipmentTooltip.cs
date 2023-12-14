using UnityEngine;
using CryptoQuest.UI.Tooltips.Equipment;
using CryptoQuest.BlackSmith.Upgrade.Presenters;
using CryptoQuest.Battle.ScriptableObjects;

namespace CryptoQuest.BlackSmith.Upgrade.UI
{
    public class UIUpgradeEquipmentTooltip : UIEquipmentTooltip
    {
        [SerializeField] private EquipmentDetailsPresenter _equipmentProvider;

        public string LevelText => _levelTextFormat;

        protected override bool CanShow()
        {
            if (_equipmentProvider == null) return false;
            if (_equipmentProvider.Equipment == null || !_equipmentProvider.Equipment.IsValid())
                return false;
            _equipment = _equipmentProvider.Equipment;
            return true;
        }

        protected override void SetupStats()
        {
            DestroyImmediatelyChilds(_statsContainer);
            base.SetupStats();
        }

        protected override void SetupSkills(RectTransform skillsContainer, ESkillType skillType, GameObject skillPrefab,
            GameObject label = null)
        {
            DestroyImmediatelyChilds(skillsContainer);
            base.SetupSkills(skillsContainer, skillType, skillPrefab, label);
        }

        // Because this is UI and the data update immediatetly so I have to DestroyImmediate
        // TODO: might refactor using object pool late for better performance
        private void DestroyImmediatelyChilds(Transform parent)
        {
            while (parent.childCount > 0)
            {
                var child = parent.GetChild(0);
                DestroyImmediate(child.gameObject);
            }
        }
    }
}