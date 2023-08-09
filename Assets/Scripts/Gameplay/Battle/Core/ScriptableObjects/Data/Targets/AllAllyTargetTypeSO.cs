using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data.Targets
{
    public class AllAllyTargetTypeSO : BattleTargetTypeSO
    {
        public override BattleTargetType GetTargetType(IBattleUnit unit, BattlePanelController battlePanelController,
            CharacterList characterList) => new AllAllyTargetType(unit, battlePanelController, characterList);
    }

    public class AllAllyTargetType : BattleTargetType
    {
        public AllAllyTargetType(IBattleUnit unit, BattlePanelController battlePanelController,
            CharacterList characterList) : base(unit, battlePanelController, characterList) { }

        public override void HandleTargets()
        {
            _unit.UnitLogic.SelectAllAlly();
        }
    }
}