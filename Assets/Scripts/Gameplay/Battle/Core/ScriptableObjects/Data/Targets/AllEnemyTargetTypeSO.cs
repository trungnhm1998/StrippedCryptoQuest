using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data.Targets
{
    public class AllEnemyTargetTypeSO : BattleTargetTypeSO
    {
        public override BattleTargetType GetTargetType(IBattleUnit unit, BattlePanelController battlePanelController)
            => new AllEnemyTargetType(unit, battlePanelController);
    }

    public class AllEnemyTargetType : BattleTargetType
    {
        public AllEnemyTargetType(IBattleUnit unit, BattlePanelController battlePanelController)
            : base(unit, battlePanelController) { }

        public override void HandleTargets()
        {
            _unit.UnitLogic.SelectAllOpponent();
        }
    }
}