using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle;
using CryptoQuest.UI.Battle.MenuStateMachine;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data.Targets
{
    [CreateAssetMenu(fileName = "OneEnemyTargetTypeSO", menuName = "Gameplay/Battle/TargetTypes/One Enemy Target Type")]
    public class OneEnemyTargetTypeSO : BattleTargetTypeSO
    {
        public override BattleTargetType GetTargetType(IBattleUnit unit, BattlePanelController battlePanelController)
            => new OneEnemyTargetType(unit, battlePanelController);
    }

    public class OneEnemyTargetType : BattleTargetType
    {
        public OneEnemyTargetType(IBattleUnit unit, BattlePanelController battlePanelController)
            : base(unit, battlePanelController) { }

        public override void HandleTargets()
        {
            _battlePanelController.BattleMenuFSM.RequestStateChange(BattleMenuStateMachine.SelectSingleEnemyState);
        }
    }
}