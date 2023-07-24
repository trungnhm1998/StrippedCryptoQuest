using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    public class UIItemsPanel : AbstractBattlePanelContent
    {
        public GameObject content;
        [SerializeField] private UIItemContent _itemPrefab;
        [SerializeField] private MockBattleItemBus attackBus;

        private List<UIItemContent> childButton= new();

        public override void Init()
        {
            foreach (var skillName in attackBus.Mobs)
            {
                var item = Instantiate(_itemPrefab, content.transform);
                childButton.Add(item);

                item.Init(new UIItemContent.Item()
                {
                    itemname = skillName,
                });
            }
        }

        public void SetActive(bool isActive)
        {
            content.SetActive(isActive);
            if (!isActive) return;

            var firstButton = childButton[0];
            firstButton.GetComponent<Button>().Select();
        }
    }
}