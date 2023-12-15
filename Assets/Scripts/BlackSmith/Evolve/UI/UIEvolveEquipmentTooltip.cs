using CryptoQuest.Battle.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Tooltips.Equipment;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIEvolveEquipmentTooltip : UIEquipmentTooltip
    {
        [SerializeField] private string _levelPreviewTextFormat = "Lv. {0}/<color=#44dfb2>{1}";

        public void SetEquipment(IEquipment equipment) => _equipment = equipment;

        protected override bool CanShow()
        {
            if (_equipment == null || !_equipment.IsValid())
                return false;

            return true;
        }

        protected override void SetupStats()
        {
            DestroyImmediatelyChilds(_statsContainer);
            base.SetupStats();
        }

        protected override void SetupSkills(RectTransform skillsContainer, ESkillType skillType, GameObject skillPrefab,
            GameObject holder)
        {
            DestroyImmediatelyChilds(skillsContainer);
            base.SetupSkills(skillsContainer, skillType, skillPrefab, holder);
        }

        // TODO: link to `UIUpgradeEquipmentTooltip`
        private void DestroyImmediatelyChilds(Transform parent)
        {
            while (parent.childCount > 0)
            {
                var child = parent.GetChild(0);
                DestroyImmediate(child.gameObject);
            }
        }

        public void SetPreviewLevel(int currentLevel, int maxLevel)
        {
            _lvl.text = string.Format(_levelPreviewTextFormat, currentLevel, maxLevel);
        }
    }
}