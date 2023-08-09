using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data.Targets
{
    public class AllEnemyTargetTypeSO : BattleTargetTypeSO
    {
        public override BattleTargetType GetTargetType(IBattleUnit unit, BattlePanelController battlePanelController,
            CharacterList characterList) => new AllEnemyTargetType(unit, battlePanelController, characterList);
    }

    public class AllEnemyTargetType : BattleTargetType
    {
        public AllEnemyTargetType(IBattleUnit unit, BattlePanelController battlePanelController,
            CharacterList characterList) : base(unit, battlePanelController, characterList) { }

        public override void HandleTargets()
        {
            if (!_unit.Owner.TryGetComponent<BattleUnitBase>(out var unitBase)) return;
            unitBase.SelectAllOpponent();
        }
    }
}