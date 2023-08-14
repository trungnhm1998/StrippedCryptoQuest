using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle;
using CryptoQuest.UI.Battle.CommandsMenu;
using CryptoQuest.UI.Battle.MenuStateMachine;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data.Targets
{
    public class EnemyGroupTargetTypeSO : BattleTargetTypeSO
    {
        public override BattleTargetType GetTargetType(IBattleUnit unit, BattlePanelController battlePanelController)
            => new EnemyGroupTargetType(unit, battlePanelController);
    }

    public class EnemyGroupTargetType : BattleTargetType
    {

        public EnemyGroupTargetType(IBattleUnit unit, BattlePanelController battlePanelController)
            : base(unit, battlePanelController) { }

        public override void HandleTargets()
        {
            _battlePanelController.BattleMenuFSM.RequestStateChange(BattleMenuStateMachine.SelectEnemyGroupState);
        }
    }
}