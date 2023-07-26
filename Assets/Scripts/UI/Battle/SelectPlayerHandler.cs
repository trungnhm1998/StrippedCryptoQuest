using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public class SelectPlayerHandler : BattleActionHandler.BattleActionHandler
    {
        [SerializeField] private BattlePanelController _panelController;
        [SerializeField] private CharacterList _characterList;

        private IBattleUnit _currentUnit;
        private readonly List<AbstractButtonInfo> _buttonInfo = new();

        private void SelectTarget(IBattleUnit unit)
        {
            _characterList.SetSelectedData(unit.UnitData.DisplayName);
            _characterList.SelectFirstHero();
        }

        public override void Handle(IBattleUnit currentUnit)
        {
            _currentUnit = currentUnit;
            SetupTargetButton(_currentUnit);
        }

        private void SetupTargetButton(IBattleUnit battleUnit)
        {
            _buttonInfo.Clear();
            var targetUnits = battleUnit.OpponentTeam.BattleUnits;

            foreach (var unit in targetUnits)
            {
                var buttonInfo = new SkillAbstractButtonInfo(unit, SelectTarget);
                _buttonInfo.Add(buttonInfo);
            }

            _panelController.OpenCommandDetailPanel(_buttonInfo);
        }
    }
}