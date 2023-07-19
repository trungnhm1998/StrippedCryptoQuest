using UnityEngine;
using IndiGames.GameplayAbilitySystem.FSM;

namespace IndiGames.GameplayAbilitySystem.Sample
{
    [CreateAssetMenu(fileName = "BattleStateSO", menuName = "Indigames Ability System/FSM/States/Battle State")]
    public class BattleStateSO : StateSO
    {
        protected BattleManager _battleManager;
        public BattleManager BattleManager => _battleManager;

        public override void OnEnterState(BaseStateMachine stateMachine)
        {
            var manager = stateMachine.GetComponent<BattleManager>();
            if (manager == null) return;
            
            _battleManager = manager;
        }
    }
}