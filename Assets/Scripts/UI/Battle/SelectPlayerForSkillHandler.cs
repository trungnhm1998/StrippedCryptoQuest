using System.Collections.Generic;
using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public class SelectPlayerForSkillHandler : BattleActionHandler.BattleActionHandler
    {
        [SerializeField] private BattlePanelController _panelController;

        private IBattleUnit _currentUnit;
        private readonly List<AbstractButtonInfo> _buttonInfo = new();

        private void SelectTarget(IBattleUnit unit)
        {
            unit.SelectSingleTarget(_currentUnit.Owner);
        }

        public override void Handle(IBattleUnit currentUnit)
        {
            _currentUnit = currentUnit;
            SetupTargetButton(_currentUnit);
        }

        private void SetupTargetButton(IBattleUnit battleUnit)
        {
            _buttonInfo.Clear();
            //TODO: get all allies then loop
            var buttonInfo = new SkillAbstractButtonInfo(battleUnit, SelectTarget);
            _buttonInfo.Add(buttonInfo);

            _panelController.OpenCommandDetailPanel(_buttonInfo);
        }
    }
}