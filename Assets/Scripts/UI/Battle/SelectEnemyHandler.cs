﻿using System.Collections.Generic;
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

        public void SelectTarget(IBattleUnit unit)
        {
            _currentUnit.SelectSingleTarget(unit.Owner);
            _panelController.CloseCommandDetailPanel();
            base.Handle(unit);
        }

        public override void Handle(IBattleUnit currentUnit)
        {
            _currentUnit = currentUnit;
            SetupTargetButton(_currentUnit);
        }

        private void SetupTargetButton(IBattleUnit battleUnit)
        {
            var buttonInfos = new List<ButtonInfo>();
            var targetUnits = battleUnit.OpponentTeam.BattleUnits;

            foreach (var unit in targetUnits)
            {
                var buttonInfo = new ButtonInfo(unit);
                buttonInfo.Clicked = delegate { SelectTarget(unit); };
                buttonInfos.Add(buttonInfo);
            }

            _panelController.OpenCommandDetailPanel(buttonInfos);
        }
    }
}