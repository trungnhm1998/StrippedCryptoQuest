using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.UI.Battle;
using CryptoQuest.UI.Battle.MenuStateMachine;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data.Targets
{
    [CreateAssetMenu(fileName = "OneAllyTargetTypeSO", menuName = "Gameplay/Battle/TargetTypes/OneAllyTargetTypeSO")]
    public class OneAllyTargetTypeSO : BattleTargetTypeSO
    {
        public override BattleTargetType GetTargetType(IBattleUnit unit, BattlePanelController battlePanelController)
            => new OneAllyTargetType(unit, battlePanelController);
    }

    public class OneAllyTargetType : BattleTargetType
    {
        public OneAllyTargetType(IBattleUnit unit, BattlePanelController battlePanelController)
            : base(unit, battlePanelController) { }

        public override void HandleTargets()
        {
            _battlePanelController.BattleMenuFSM.RequestStateChange(BattleMenuStateMachine.SelectHeroState);
        }
    }
}