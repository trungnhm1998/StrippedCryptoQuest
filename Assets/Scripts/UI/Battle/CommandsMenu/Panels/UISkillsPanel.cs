using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    public class UISkillsPanel : AbstractBattlePanelContent
    {
        public GameObject content;
        [SerializeField] private UISkillContent _itemPrefab;
        [SerializeField] private MockBattleSkillBus attackBus;

        private List<UISkillContent> childButton = new();

        public override void Init()
        {
            for (var index = 0; index < attackBus.Skill.Length; index++)
            {
                var skillName = attackBus.Skill[index];
                var item = Instantiate(_itemPrefab, content.transform);
                childButton.Add(item);

                item.Init(new UISkillContent.Skill()
                {
                    skillname = skillName,
                });
            }
        }

        public override void SetPanelActive(bool isActive)
        {
            content.SetActive(isActive);
            if (!isActive) return;

            var firstButton = childButton[0];
            firstButton.GetComponent<Button>().Select();
        }
    }
}