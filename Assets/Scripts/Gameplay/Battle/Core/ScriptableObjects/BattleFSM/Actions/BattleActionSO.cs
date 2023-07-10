using UnityEngine;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.FSM;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "BattleActionSO", menuName = "Gameplay/Battle/FSM/Actions/Battle Action")]
    public class BattleActionSO : FSMActionSO
    {
        private BattleManager _manager;
        public BattleManager Manager => _manager;

        public override void Execute(BaseStateMachine stateMachine)
        {
            if (_manager != null) return;
            _manager = stateMachine.GetComponent<BattleManager>();
        }
    }
}