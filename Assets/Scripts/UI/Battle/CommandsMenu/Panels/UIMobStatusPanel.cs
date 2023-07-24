using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    public class UIMobStatusPanel : AbstractBattlePanelContent
    {
        public GameObject content;
        [SerializeField] private UIMobContent _itemPrefab;
        [SerializeField] private MockBattleMobBus attackBus;

        private List<UIMobContent> childButton = new();

        public override void Init()
        {
            foreach (var skillName in attackBus.Mobs)
            {
                var item = Instantiate(_itemPrefab, content.transform);

                childButton.Add(item);
                item.Init(new UIMobContent.Attack()
                {
                    mob = skillName,
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