using UnityEngine;
using IndiGames.GameplayAbilitySystem.FSM;

namespace IndiGames.GameplayAbilitySystem.Sample
{
    [CreateAssetMenu(fileName = "BattleActionSO", menuName = "Indigames Ability System/FSM/Actions/Battle Action")]
    public class BattleActionSO : FSMActionSO
    {
        private BattleManager _manager;
        public BattleManager Manager => _manager;

        public override void Execute(BaseStateMachine stateMachine)
        {
            if (_manager != null) return;

            var manager = stateMachine.GetComponent<BattleManager>();
            if (manager == null) return;
            
            _manager = manager;
        }
    }
}