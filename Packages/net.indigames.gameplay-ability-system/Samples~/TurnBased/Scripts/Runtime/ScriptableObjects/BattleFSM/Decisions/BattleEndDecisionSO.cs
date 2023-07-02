using UnityEngine;
using Indigames.AbilitySystem.FSM;

namespace Indigames.AbilitySystem.Sample
{
    [CreateAssetMenu(fileName = "BattleEndDecisionSO", menuName = "Indigames Ability System/FSM/Decisions/Battle End Dicision")]
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