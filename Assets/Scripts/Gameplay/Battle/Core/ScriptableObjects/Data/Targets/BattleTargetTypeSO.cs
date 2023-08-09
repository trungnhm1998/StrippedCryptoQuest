using CryptoQuest.Gameplay.BaseGameplayData;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data.Targets
{
    public abstract class BattleTargetTypeSO : GenericData
    {
        public abstract BattleTargetType GetTargetType(IBattleUnit unit, BattlePanelController battlePanelController,
            CharacterList characterList);
    }

    public abstract class BattleTargetType
    {
        protected IBattleUnit _unit;
        protected BattlePanelController _battlePanelController;
        protected CharacterList _characterList;

        public abstract void HandleTargets();

        public BattleTargetType(IBattleUnit unit, BattlePanelController battlePanelController,
            CharacterList characterList)
        {
            _unit = unit;
            _battlePanelController = battlePanelController;
            _characterList = characterList;
        }
    }
}