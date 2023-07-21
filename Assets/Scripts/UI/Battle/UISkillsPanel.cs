using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Battle
{
    public class UISkillsPanel : AbstractBattlePanelContent
    {
        public GameObject content;
        [SerializeField] private UISkillContent _itemPrefab;
        [SerializeField] private MockBattleSkillBus attackBus;

        public override void Init()
        {
            foreach (var skillName in attackBus.Skill)
            {
                var item = Instantiate(_itemPrefab, content.transform);
                item.Init(new UISkillContent.Skill()
                {
                    skillname = skillName,
                });
            }
        }
    }
}