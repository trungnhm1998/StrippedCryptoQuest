using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;

namespace CryptoQuest.UI.Battle.BattleActionHandler
{
    public class SelectEnemyHandler : BattleActionHandler
    {
        [SerializeField] private BattlePanelController _panelController;

        private IBattleUnit _currentUnit;

        public void OnSelectTarget(IBattleUnit unit)
        {
            _currentUnit.SelectSingleTarget(unit.Owner);
            _panelController.CloseCommandDetailPanel();
            base.Handle(unit);
        }

        public override object Handle(IBattleUnit currentUnit)
        {
            _currentUnit = currentUnit;
            if (_currentUnit == null) return currentUnit;
            SetupTargetButton(_currentUnit.OpponentTeam);
            return null;
        }

        private void SetupTargetButton(BattleTeam team)
        {
            var buttonInfos = new List<ButtonInfo>();
            var targetUnits = team.BattleUnits;

            foreach (var unit in targetUnits)
            {
                var buttonInfo = new ButtonInfo()
                {
                    Name = unit.UnitData.DisplayName,
                    Value = "",
                    Callback = () => OnSelectTarget(unit)
                };
                buttonInfos.Add(buttonInfo);
            }

            _panelController.OpenCommandDetailPanel(buttonInfos);
        }
    }
}