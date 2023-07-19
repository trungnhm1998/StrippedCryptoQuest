using CryptoQuest.FSM;
using CryptoQuest.FSM.ScriptableObjects.Base;
using CryptoQuest.Gameplay.Battle.Core.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.BattleFSM.Decisions
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