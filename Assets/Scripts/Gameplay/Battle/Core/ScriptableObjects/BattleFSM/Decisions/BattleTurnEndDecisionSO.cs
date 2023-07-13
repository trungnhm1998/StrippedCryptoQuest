using UnityEngine;
using CryptoQuest.FSM;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "BattleTurnEndDecisionSO", menuName = "Gameplay/Battle/FSM/Decisions/Battle Turn End Dicision")]
    public class BattleTurnEndDecisionSO : DecisionSO
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            if (!stateMachine.TryGetComponent<BattleManager>(out var battleManager)) return false;            
            return battleManager.IsEndTurn;
        }
    }
}