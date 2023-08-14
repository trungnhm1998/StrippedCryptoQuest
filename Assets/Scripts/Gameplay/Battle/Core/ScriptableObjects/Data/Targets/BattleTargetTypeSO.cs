using CryptoQuest.Gameplay.BaseGameplayData;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data.Targets
{
    public abstract class BattleTargetTypeSO : GenericData
    {
        public abstract BattleTargetType GetTargetType(IBattleUnit unit, BattlePanelController battlePanelController);
    }

    public abstract class BattleTargetType
    {
        protected IBattleUnit _unit;
        protected BattlePanelController _battlePanelController;

        public abstract void HandleTargets();

        public BattleTargetType(IBattleUnit unit, BattlePanelController battlePanelController)
        {
            _unit = unit;
            _battlePanelController = battlePanelController;
        }
    }
}