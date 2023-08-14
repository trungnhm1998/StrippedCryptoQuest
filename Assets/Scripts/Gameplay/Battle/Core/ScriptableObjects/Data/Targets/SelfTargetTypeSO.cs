using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data.Targets
{
    public class SelfTargetTypeSO : BattleTargetTypeSO
    {
        public override BattleTargetType GetTargetType(IBattleUnit unit, BattlePanelController battlePanelController)
            => new SelfTargetType(unit, battlePanelController);
    }

    public class SelfTargetType : BattleTargetType
    {
        public SelfTargetType(IBattleUnit unit, BattlePanelController battlePanelController)
            : base(unit, battlePanelController) { }

        public override void HandleTargets()
        {
            _unit.SelectSingleTarget(_unit.Owner);
        }
    }
}