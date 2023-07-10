using UnityEngine;
using CryptoQuest.FSM;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "BattleEndDecisionSO", menuName = "Gameplay/Battle/FSM/Decisions/Battle End Dicision")]
    public class BattleEndDecisionSO : DecisionSO
    {
        public override bool Decide(BaseStateMachine stateMachine)
        {
            var battleManager = stateMachine.GetComponent<BattleManager>();
            if (battleManager == null) return false;
            
            return battleManager.IsBattleEnd();
        }
    }
}