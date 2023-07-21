using System;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public class UIMobStatusPanel : AbstractBattlePanelContent
    {
        public GameObject content;
        [SerializeField] private UIMobContent _itemPrefab;
        [SerializeField] private MockBattleMobBus attackBus;

        public override void Init()
        {
            foreach (var skillName in attackBus.Mobs)
            {
                var item = Instantiate(_itemPrefab, content.transform);
                item.Init(new UIMobContent.Attack()
                {
                    mob = skillName, 
                });
            }
        }
    }
}