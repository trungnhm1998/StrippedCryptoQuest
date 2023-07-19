using CryptoQuest.FSM;
using CryptoQuest.FSM.ScriptableObjects.Base;
using CryptoQuest.Gameplay.Battle.Core.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.BattleFSM.Decisions
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