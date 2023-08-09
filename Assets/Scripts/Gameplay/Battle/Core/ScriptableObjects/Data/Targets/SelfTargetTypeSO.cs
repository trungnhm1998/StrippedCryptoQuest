using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data.Targets
{
    public class SelfTargetTypeSO : BattleTargetTypeSO
    {
        public override BattleTargetType GetTargetType(IBattleUnit unit, BattlePanelController battlePanelController,
            CharacterList characterList)
            => new SelfTargetType(unit, battlePanelController, characterList);
    }

    public class SelfTargetType : BattleTargetType
    {
        public SelfTargetType(IBattleUnit unit, BattlePanelController battlePanelController,
            CharacterList characterList) : base(unit, battlePanelController, characterList) { }

        public override void HandleTargets()
        {
            _unit.SelectSingleTarget(_unit.Owner);
        }
    }
}