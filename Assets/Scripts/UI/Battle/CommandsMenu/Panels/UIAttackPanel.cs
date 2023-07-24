using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    internal class UIAttackPanel : AbstractBattlePanelContent
    {
        public GameObject content;
        [SerializeField] private UIAttackContent _itemPrefab;
        [SerializeField] private MockBattleAttackBus attackBus;

        private List<UIAttackContent> childButton = new();

        public override void Init()
        {
            foreach (var mob in attackBus.Mobs)
            {
                var item = Instantiate(_itemPrefab, content.transform);
                childButton.Add(item);
                item.Init(new UIAttackContent.Attack()
                {
                    mob = mob
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