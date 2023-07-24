using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    internal class UIAttackPanel : AbstractBattlePanelContent
    {
        public GameObject content;
        [SerializeField] private UIAttackContent _itemPrefab;

        private List<UIAttackContent> childButton = new();

        public override void Init(IBattleUnit unit)
        {
            foreach (IBattleUnit currentUnit in unit.OpponentTeam.BattleUnits)
            {
                var item = Instantiate(_itemPrefab, content.transform);
                childButton.Add(item);
                item.Init(currentUnit);
            }
        }

        public override void SetPanelActive(bool isActive)
        {
            content.SetActive(isActive);
            if (!isActive) return;
            if (childButton == null || childButton.Count == 0) return;

            var firstButton = childButton[0];
            firstButton.GetComponent<Button>().Select();
        }
    }
}