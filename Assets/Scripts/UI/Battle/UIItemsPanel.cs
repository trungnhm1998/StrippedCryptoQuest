using System;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public class UIItemsPanel : AbstractBattlePanelContent
    {
        public GameObject content;
        [SerializeField] private UIItemContent _itemPrefab;
        [SerializeField] private MockBattleItemBus attackBus;

        public override void Init()
        {
            foreach (var skillName in attackBus.Mobs)
            {
                var item = Instantiate(_itemPrefab, content.transform);
                item.Init(new UIItemContent.Item()
                {
                    itemname = skillName,
                });
            }
        }
    }
}