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

        private void SelectTarget(IBattleUnit unit)
        {
            _characterList.ShowSelected(unit.UnitData.DisplayName);
            _characterList.SelectFirstHero();
        }

        public override void Handle(IBattleUnit currentUnit)
        {
            _currentUnit = currentUnit;
            SetupTargetButton(_currentUnit);
        }

        private void SetupTargetButton(IBattleUnit battleUnit)
        {
            var buttonInfos = new List<AbstractButtonInfo>();
            var targetUnits = battleUnit.OpponentTeam.BattleUnits;

            foreach (var unit in targetUnits)
            {
                var buttonInfo = new SkillAbstractButtonInfo(unit, SelectTarget);
                buttonInfos.Add(buttonInfo);
            }

            _panelController.OpenCommandDetailPanel(buttonInfos);
        }
    }
}